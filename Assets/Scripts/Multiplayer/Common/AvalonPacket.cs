public abstract class AvalonPacket
{
    public string ClientId { get; private set; }

    public void SetClientId(string clientId)
    {
        ClientId = clientId;
    }
    // Base class for all Avalon domain packets
}