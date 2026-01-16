using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using static ServerCommunicationLayerManager;

public interface IAvalonPacketServerSender {
    void Send(AvalonPacket packet);
    void Send(AvalonPacket packet, IEnumerable<string> recipientIds);
}
