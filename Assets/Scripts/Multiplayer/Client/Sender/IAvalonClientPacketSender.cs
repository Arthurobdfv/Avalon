using UnityEngine;

public interface IAvalonClientPacketSender
{
    void Send(AvalonPacket packet);
}
