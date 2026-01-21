using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LocalServerPacketReceiver : MonoBehaviour, IAvalonPacketServerReceiver
{
    public event IAvalonPacketServerReceiver.OnPacketReveivedDelegate OnPacketReceived;

    public void Handle(AvalonPacket packet)
    {
        if (string.IsNullOrEmpty(packet.ClientId))
        {
            packet.SetClientId(LocalServerPacketSender.CurrentPlayerId);
        }
        OnPacketReceived?.Invoke(packet);
    }
}
