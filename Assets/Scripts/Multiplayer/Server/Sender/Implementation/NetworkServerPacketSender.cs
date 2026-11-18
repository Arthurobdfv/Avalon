using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkServerPacketSender : MonoBehaviour, IAvalonPacketServerSender
{
    NetworkServer _server;
    NetworkServer Server
    {
        get
        {
            if (_server == null)
            {
                _server = GameObject.FindObjectOfType<NetworkServer>();
            }
            return _server;
        }
    }
    public void Send(AvalonPacket packet)
    {
        Server.Send(SerializeAnyPacket(packet), new List<string>() { packet.ClientId });
    }

    public void Send(AvalonPacket packet, IEnumerable<string> recipientIds)
    {
        foreach (var recipient in recipientIds)
        {
            packet.SetClientId(recipient);
            Server.Send(SerializeAnyPacket(packet), new List<string>() { recipient });
        }
    }


    private byte[] SerializeAnyPacket(AvalonPacket packet)
    {
        return PacketDescriberRegistry.Serialize(packet);
    }
}
