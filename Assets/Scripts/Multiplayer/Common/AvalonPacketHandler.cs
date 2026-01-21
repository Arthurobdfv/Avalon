using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


// This class will receiva all metadata + domain packet and handle them accordingly on the server side.
public class AvalonPacketHandler : MonoBehaviour
{
    private Dictionary<Type, Action<AvalonPacket>> PacketHandlers = new Dictionary<Type, Action<AvalonPacket>>();
    // Start is called before the first frame update
    void Start()
    {
        RegisterPacketHandlers();
    }

    private void RegisterPacketHandlers()
    {
        //throw new NotImplementedException();
    }

    public void Handle(AvalonPacket packet)
    {
        var packetType = packet.GetType();
        if (PacketHandlers.TryGetValue(packetType, out var handler))
        {
            handler(packet);
        }
        else
        {
            Debug.LogWarning($"{GetType()}: No handler registered for packet type: {packetType}");
        }
    }

    protected void RegisterHandler<T>(Action<T> handler) where T : AvalonPacket
    {
        PacketHandlers[typeof(T)] = packet => handler((T)packet);
    }

    protected void UnregisterHandler<T>() where T : AvalonPacket
    {
        PacketHandlers.Remove(typeof(T));
    }
}
