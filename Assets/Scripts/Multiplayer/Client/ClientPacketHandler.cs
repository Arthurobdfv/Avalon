using System;

public class ClientPacketHandler : AvalonPacketHandler
{
    public void RegisterClientHandler<T>(Action<T> handler) where T : AvalonPacket
    {
        RegisterHandler<T>(handler);
    }

    public void UnregisterClientHandler<T>() where T : AvalonPacket
    {
        UnregisterHandler<T>();
    }
}
