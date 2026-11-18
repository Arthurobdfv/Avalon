using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkServer : MonoBehaviour
{
    private static TcpListener tcpListener;
    public static TcpListener Listener => tcpListener;

    HashSet<TcpClient> connectedClients = new HashSet<TcpClient>();

    public class AvalonClient
    {
        public Coroutine ListenCoroutine;
        byte[] receiveBuffer = new byte[MultiplayerConstants.MAX_PACKET_SIZE];
        public TcpClient tcpClient;
        public string ConnectionId;
        public string PlayerId;
        public NetworkStream Stream;
    }
    Dictionary<TcpClient, AvalonClient> clientSessions = new Dictionary<TcpClient, AvalonClient>();
    Dictionary<string, AvalonClient> AuthPlayers = new Dictionary<string, AvalonClient>();



    private Coroutine ListenAcceptCoroutine;
    public delegate void DataReceived(AvalonClient client, byte[] data);
    public static event DataReceived OnDataReceived;

    void Start()
    {
        if (tcpListener != null)
        {
            HandleServerShutDown();
        }
        tcpListener = new TcpListener(System.Net.IPAddress.Any, MultiplayerConstants.SERVER_PORT);
        Debug.Log($"Starting TCP Listener on port {MultiplayerConstants.SERVER_PORT}");
        tcpListener.Start();
        ListenAcceptCoroutine = StartCoroutine(ListenForAccepts());
    }

    private IEnumerator ListenForAccepts()
    {
        var acceptAsync = tcpListener.AcceptTcpClientAsync();
        while (!acceptAsync.IsCompleted)
        {
            yield return null;
        }
        if (acceptAsync.IsFaulted)
        {
            Debug.LogError("Failed to accept TCP client: " + acceptAsync.Exception);
            yield break;
        }
        else
        {

            TcpClient client = acceptAsync.Result;
            Debug.Log($"Accepted new client from {client.Client.RemoteEndPoint}");
            HandleConnect(client);
        }
    }

    public void FixedUpdate()
    {
        if (ListenAcceptCoroutine == null)
        {
            ListenAcceptCoroutine = StartCoroutine(ListenForAccepts());
        }
    }

    private void HandleServerShutDown()
    {
        tcpListener.Stop();
        tcpListener = null;
    }

    private void HandleConnect(TcpClient client)
    {
        connectedClients.Add(client);
        Debug.Log($"Client connected: {client.Client.RemoteEndPoint}, Starting listening...");
        var avalonClient = new AvalonClient
        {
            tcpClient = client,
            ListenCoroutine = StartCoroutine(ListenForPackets(client)),
            Stream = client.GetStream()
        };
        Debug.Log($"Started listening for client: {client.Client.RemoteEndPoint}");
        clientSessions[client] = avalonClient;
    }

    private IEnumerator ListenForPackets(TcpClient client)
    {
        var stream = client.GetStream();

        while (client.Connected)
        {
            byte[] buffer = new byte[MultiplayerConstants.MAX_PACKET_SIZE];
            int bytesRead = 0;

            var readDataAsync = stream.ReadAsync(buffer, 0, buffer.Length);
            while (!readDataAsync.IsCompleted)
            {
                yield return null;
            }
            if (readDataAsync.IsFaulted)
            {
                Debug.LogError("Failed to read data from client: " + readDataAsync.Exception);
            }
            bytesRead = readDataAsync.Result;
            if (bytesRead > 0)
            {
                OnDataReceived?.Invoke(clientSessions[client], buffer);
            }
        }

        // Clean up when the client disconnects
        //HandleDisconnect(client);   
    }

    public void Send(byte[] data, IEnumerable<string> recipients)
    {
        foreach(var recipient in recipients)
        {
            AuthPlayers[recipient].Stream.Write(data);
        }
    }

    public void Write(byte[] data, TcpClient client)
    {
        if (client.Connected)
        {
            var stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }
    }

    internal void AssignClientIdToClient(AvalonClient client, string playerId)
    {
        client.PlayerId = playerId;
        if (AuthPlayers.ContainsKey(playerId))
        {
            AuthPlayers[playerId] = client;
        }
        else
        {
            AuthPlayers.Add(playerId, client);
        }
    }
}
