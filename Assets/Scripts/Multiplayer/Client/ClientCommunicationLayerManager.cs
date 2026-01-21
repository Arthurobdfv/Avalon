using System.Linq;
using UnityEngine;

public class ClientCommunicationLayerManager : MonoBehaviour
{
    IAvalonClientPacketSender _packetSender;
    IAvalonPacketClientReceiver _packetReceiver;

    ClientPacketHandler _packetHandler;

    private void Awake()
    {
        _packetSender = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IAvalonClientPacketSender>().FirstOrDefault() ?? throw new System.NullReferenceException("No IAvalonClientPacketSender found");
        _packetHandler = FindObjectOfType<ClientPacketHandler>() ?? throw new System.NullReferenceException("No ClientPacketHandler found");
    }
    public void Send(AvalonPacket packet)
    {
        //Debug.Log($"Client sending packet of type {packet.GetType().Name}");
        _packetSender?.Send(packet);
    }

    public void Handle(AvalonPacket packet)
    {
        //Debug.Log($"Client handling packet of type {packet.GetType().Name}");
        _packetHandler?.Handle(packet);
    }
}
