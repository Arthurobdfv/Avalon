using System;
using System.Collections;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Unity MonoBehaviour that handles Google OAuth login via browser redirect.
/// Works for standalone desktop builds (Windows, macOS, Linux). Not compatible with WebGL.
/// 
/// Flow:
/// 1. Fetches OAuth config from server (client ID, endpoints)
/// 2. Opens browser directly to Google authorization URL
/// 3. Receives authorization code via loopback redirect
/// 4. Sends code to server for exchange (server handles client secret securely)
/// 5. Receives app JWT from server
/// </summary>
public class LocalOAuthClient : MonoBehaviour
{
    [Header("OAuth Settings")]
    [Tooltip("Base URL of your AvalonAuthServer API")]
    [SerializeField] private string apiBaseUrl = "http://localhost:5151";

    [Tooltip("Local port for receiving the OAuth callback")]
    [SerializeField] private int localCallbackPort = 7890;

    [Header("State")]
    [SerializeField] private string currentToken;
    [SerializeField] private bool isAuthenticating;

    private HttpListener _httpListener;
    private Thread _listenerThread;
    private string _pendingCode;
    private string _pendingError;
    private bool _callbackReceived;

    // OAuth config from server
    private string _googleClientId;
    private string _googleAuthEndpoint;
    private string _googleScope;

    public string Token => currentToken;
    public bool IsAuthenticated => !string.IsNullOrEmpty(currentToken);
    public bool IsAuthenticating => isAuthenticating;

    public event Action<string> OnLoginSuccess;
    public event Action<string> OnLoginFailed;

    private string RedirectUri => $"http://127.0.0.1:{localCallbackPort}/";

    private void Update()
    {
        // Process callback result on main thread (required for Unity events/UI)
        if (_callbackReceived)
        {
            _callbackReceived = false;

            if (!string.IsNullOrEmpty(_pendingCode))
            {
                Debug.Log("[LocalOAuthClient] Authorization code received, exchanging for token...");
                StartCoroutine(ExchangeCodeForToken(_pendingCode));
            }
            else
            {
                isAuthenticating = false;
                Debug.LogError($"[LocalOAuthClient] Login failed: {_pendingError}");
                OnLoginFailed?.Invoke(_pendingError);
                StopListener();
            }

            _pendingCode = null;
            _pendingError = null;
        }
    }

    private void OnDestroy()
    {
        StopListener();
    }

    /// <summary>
    /// Initiates the Google OAuth login flow.
    /// Opens the system browser for user to sign in.
    /// </summary>
    public void StartGoogleLogin()
    {
        if (isAuthenticating)
        {
            Debug.LogWarning("[LocalOAuthClient] Authentication already in progress.");
            return;
        }

        isAuthenticating = true;
        currentToken = null;

        StartCoroutine(FetchConfigAndStartLogin());
    }

    /// <summary>
    /// Cancels an in-progress authentication attempt.
    /// </summary>
    public void CancelLogin()
    {
        if (!isAuthenticating) return;

        isAuthenticating = false;
        StopListener();
        Debug.Log("[LocalOAuthClient] Login cancelled by user.");
    }

    /// <summary>
    /// Clears the current token (logout).
    /// </summary>
    public void Logout()
    {
        currentToken = null;
        Debug.Log("[LocalOAuthClient] Logged out.");
    }

    private IEnumerator FetchConfigAndStartLogin()
    {
        // Fetch OAuth config from server
        var configUrl = $"{apiBaseUrl}/api/oauth2/google/config";
        Debug.Log($"[LocalOAuthClient] Fetching OAuth config from {configUrl}");

        using (var request = UnityWebRequest.Get(configUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                isAuthenticating = false;
                var error = $"Failed to fetch OAuth config: {request.error}";
                Debug.LogError($"[LocalOAuthClient] {error}");
                OnLoginFailed?.Invoke(error);
                yield break;
            }

            try
            {
                var config = JsonUtility.FromJson<GoogleOAuthConfig>(request.downloadHandler.text);
                _googleClientId = config.clientId;
                _googleAuthEndpoint = config.authorizationEndpoint;
                _googleScope = config.scope;
            }
            catch (Exception ex)
            {
                isAuthenticating = false;
                var error = $"Failed to parse OAuth config: {ex.Message}";
                Debug.LogError($"[LocalOAuthClient] {error}");
                OnLoginFailed?.Invoke(error);
                yield break;
            }
        }

        // Start listener and open browser
        try
        {
            StartListener();
            OpenBrowserForLogin();
        }
        catch (Exception ex)
        {
            isAuthenticating = false;
            Debug.LogError($"[LocalOAuthClient] Failed to start login: {ex.Message}");
            OnLoginFailed?.Invoke(ex.Message);
        }
    }

    private IEnumerator ExchangeCodeForToken(string code)
    {
        StopListener();

        var exchangeUrl = $"{apiBaseUrl}/api/oauth2/google/exchange";
        var requestBody = JsonUtility.ToJson(new GoogleCodeExchangeRequest
        {
            code = code,
            redirectUri = RedirectUri
        });

        Debug.Log($"[LocalOAuthClient] Exchanging code at {exchangeUrl}");

        using (var request = new UnityWebRequest(exchangeUrl, "POST"))
        {
            var bodyBytes = Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            isAuthenticating = false;

            if (request.result != UnityWebRequest.Result.Success)
            {
                var error = $"Token exchange failed: {request.error} - {request.downloadHandler.text}";
                Debug.LogError($"[LocalOAuthClient] {error}");
                OnLoginFailed?.Invoke(error);
                yield break;
            }

            try
            {
                var response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
                currentToken = response.token;
                Debug.Log($"[LocalOAuthClient] Login successful! User: {response.username}");
                OnLoginSuccess?.Invoke(currentToken);
            }
            catch (Exception ex)
            {
                var error = $"Failed to parse auth response: {ex.Message}";
                Debug.LogError($"[LocalOAuthClient] {error}");
                OnLoginFailed?.Invoke(error);
            }
        }
    }

    private void StartListener()
    {
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add(RedirectUri);
        _httpListener.Start();

        _listenerThread = new Thread(ListenForCallback)
        {
            IsBackground = true
        };
        _listenerThread.Start();

        Debug.Log($"[LocalOAuthClient] Listening for OAuth callback on {RedirectUri}");
    }

    private void StopListener()
    {
        try
        {
            _httpListener?.Stop();
            _httpListener?.Close();
        }
        catch
        {
            // Ignore errors during cleanup
        }

        _httpListener = null;
        _listenerThread = null;
    }

    private void ListenForCallback()
    {
        try
        {
            // Wait for the browser redirect
            var context = _httpListener.GetContext();
            var request = context.Request;

            // Extract authorization code from query string
            var code = request.QueryString["code"];
            var error = request.QueryString["error"];

            // Send response to browser
            var response = context.Response;
            string responseHtml;

            if (!string.IsNullOrEmpty(code))
            {
                _pendingCode = code;
                responseHtml = @"
                    <html>
                    <head><title>Login Successful</title></head>
                    <body style='font-family: Arial, sans-serif; text-align: center; padding: 50px;'>
                        <h1 style='color: #4CAF50;'>&#10004; Authorization Received!</h1>
                        <p>Completing login... You can close this window and return to the application.</p>
                    </body>
                    </html>";
            }
            else
            {
                _pendingError = error ?? "No authorization code received";
                responseHtml = $@"
                    <html>
                    <head><title>Login Failed</title></head>
                    <body style='font-family: Arial, sans-serif; text-align: center; padding: 50px;'>
                        <h1 style='color: #f44336;'>&#10008; Login Failed</h1>
                        <p>{_pendingError}</p>
                        <p>Please close this window and try again.</p>
                    </body>
                    </html>";
            }

            var buffer = Encoding.UTF8.GetBytes(responseHtml);
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.Close();

            // Signal main thread
            _callbackReceived = true;
        }
        catch (HttpListenerException)
        {
            // Listener was stopped (cancelled or destroyed)
        }
        catch (ObjectDisposedException)
        {
            // Listener was disposed
        }
        catch (Exception ex)
        {
            _pendingError = ex.Message;
            _callbackReceived = true;
        }
    }

    private void OpenBrowserForLogin()
    {
        // Generate state for CSRF protection
        var state = Guid.NewGuid().ToString("N");

        // Build Google OAuth URL directly
        var authUrl = $"{_googleAuthEndpoint}" +
            $"?client_id={Uri.EscapeDataString(_googleClientId)}" +
            $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
            $"&response_type=code" +
            $"&scope={Uri.EscapeDataString(_googleScope)}" +
            $"&state={state}" +
            $"&access_type=offline";

        Debug.Log($"[LocalOAuthClient] Opening browser: {authUrl}");
        Application.OpenURL(authUrl);
    }

    #region JSON Models

    [Serializable]
    private class GoogleOAuthConfig
    {
        public string clientId;
        public string authorizationEndpoint;
        public string tokenEndpoint;
        public string scope;
    }

    [Serializable]
    private class GoogleCodeExchangeRequest
    {
        public string code;
        public string redirectUri;
    }

    [Serializable]
    private class AuthResponse
    {
        public string token;
        public string username;
        public string email;
        public string expiresAt;
    }

    #endregion
}
