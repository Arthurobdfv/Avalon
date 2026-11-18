using System;
using UnityEngine;

public class NetworkServerPacketReceiver : MonoBehaviour, IAvalonPacketServerReceiver
{
    public event IAvalonPacketServerReceiver.OnPacketReveivedDelegate OnPacketReceived;
    public NetworkServer Server
    {
        get
        {
            if (_server == null)
            {
                _server = GameObject.FindObjectOfType<NetworkServer>();
            }
            return _server;
        }
    }

    private AvalonAuthServer _authServer;
    AvalonAuthServer AuthServer
    {
        get
        {
            if (_authServer == null)
            {
                _authServer = GameObject.FindObjectOfType<AvalonAuthServer>();
            }
            return _authServer;
        }
    }

    private void Start()
    {
        NetworkServer.OnDataReceived += OnNetworkDataReceived;
    }

    private void OnNetworkDataReceived(NetworkServer.AvalonClient client, byte[] data)
    {
        AvalonPacket packet = DeserializeAnyPacket(data);
        if (packet != null)
        {
            if(packet.GetType() == typeof(ConnectPlayerPacket) || packet.GetType() == typeof(ConnectObserverPacket))
            {
                var connectionPacket = (ConnectPlayerPacket)packet;
                AvalonAuthServer.Authorize(connectionPacket.PlayerId, connectionPacket.AuthToken);
                AvalonAuthServer.Connect(connectionPacket.PlayerId, connectionPacket.AuthToken);
                Server.AssignClientIdToClient(client, connectionPacket.PlayerId);
            }
            packet.SetClientId(client.PlayerId);
            Handle(packet);
        }
    }

    private NetworkServer _server;
    public void Handle(AvalonPacket packet)
    {
        OnPacketReceived?.Invoke(packet);
    }

    private AvalonPacket DeserializeAnyPacket(byte[] data)
    {
        return PacketDescriberRegistry.Deserialize(data);
    }

}
