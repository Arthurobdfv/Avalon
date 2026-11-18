using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerClientPacketReceiver : MonoBehaviour, IAvalonPacketClientReceiver
{
    NetworkClient _networkClient;

    public event IAvalonPacketClientReceiver.OnPacketReceivedDelegate OnPacketReceived;

    NetworkClient NetworkClient
    {
        get
        {
            if(_networkClient == null)
            {
                _networkClient = FindFirstObjectByType<NetworkClient>() ?? throw new System.NullReferenceException("No NetworkClient found");
            }
            return _networkClient;
        }
    }

    public void Handle(AvalonPacket packet)
    {
        OnPacketReceived?.Invoke(packet);
    }

    // Start is called before the first frame update
    void Start()
    {
        NetworkClient.OnDataReceived += HandleDataReceived;
    }

    void OnDestroy()
    {
        NetworkClient.OnDataReceived -= HandleDataReceived;
    }

    private void HandleDataReceived(byte[] data)
    {
        AvalonPacket packet = DeserializeAnyPacket(data);
        Handle(packet);
    }

    private AvalonPacket DeserializeAnyPacket(byte[] data)
    {
        return PacketDescriberRegistry.Deserialize(data);
    }
}
