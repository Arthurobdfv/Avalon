public interface IAvalonPacketServerReceiver 
{
    public void Handle(AvalonPacket packet);
    public delegate void OnPacketReveivedDelegate(AvalonPacket packet);
    public event OnPacketReveivedDelegate OnPacketReceived;
}
