// This class will receiva all metadata + domain packet and handle them accordingly on the server side.
using System;

public class ServerPacketHandler : AvalonPacketHandler
{
    public void RegisterServerHandler<T>(Action<T> handler) where T : AvalonPacket
    {
        RegisterHandler<T>(handler);
    }

    public void UnregisterServerHandler<T>() where T : AvalonPacket
    {
        UnregisterHandler<T>();
    }
}
