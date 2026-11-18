public interface IAvalonPacketClientReceiver
{
    void Handle(AvalonPacket packet);
    public delegate void OnPacketReceivedDelegate(AvalonPacket packet);
    public event OnPacketReceivedDelegate OnPacketReceived;
}
