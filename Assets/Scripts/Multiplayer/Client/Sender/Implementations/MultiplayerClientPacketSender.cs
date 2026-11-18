using UnityEngine;

public class MultiplayerClientPacketSender : MonoBehaviour, IAvalonClientPacketSender
{
    NetworkClient _networkClient;
    NetworkClient NetworkClient 
    { 
        get 
        { 
            if (_networkClient == null) 
            { 
                _networkClient = GameObject.FindObjectOfType<NetworkClient>(); 
            } 
            return _networkClient; 
        } 
    }
    public void Send(AvalonPacket packet)
    {
        NetworkClient.Send(SerializeAnyPacket(packet));
    }

    private byte[] SerializeAnyPacket(AvalonPacket packet)
    {
        return PacketDescriberRegistry.Serialize(packet);
    }
}
