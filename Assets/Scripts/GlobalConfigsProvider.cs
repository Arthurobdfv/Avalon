using UnityEngine;

public class GlobalConfigsProvider : MonoBehaviour
{
    [SerializeField] private GlobalConfigs _configs;

    private static bool _multiplayerEnabled = false;
    public static bool MultiplayerEnabled { get => _multiplayerEnabled; private set => _multiplayerEnabled = value; }

    private void Update()
    {
        if(MultiplayerEnabled != _configs.MultiplayerEnabled)
        {
            MultiplayerEnabled = _configs.MultiplayerEnabled;
            MultiplayerEnabledChanged?.Invoke(MultiplayerEnabled);
        }
    }

    public delegate void OnMultiplayerEnabledChanged(bool isEnabled);
    public static event OnMultiplayerEnabledChanged MultiplayerEnabledChanged;
}
