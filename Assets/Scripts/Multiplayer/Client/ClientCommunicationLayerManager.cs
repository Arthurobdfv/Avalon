using System.Linq;
using UnityEngine;

public class ClientCommunicationLayerManager : MonoBehaviour
{
    IAvalonClientPacketSender _packetSender;
    IAvalonPacketClientReceiver _packetReceiver;

    static ClientPacketHandler _packetHandler = new();

    IAvalonClientPacketSender PacketSender
    {
        get
        {
            if (_packetSender == null)
            {
                _packetSender = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IAvalonClientPacketSender>().FirstOrDefault() ?? throw new System.NullReferenceException("No IAvalonClientPacketSender found");
            }
            return _packetSender;
        }
    }

    IAvalonPacketClientReceiver PacketReceiver
    {
        get
        {
            if (_packetReceiver == null)
            {
                _packetReceiver = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IAvalonPacketClientReceiver>().FirstOrDefault() ?? throw new System.NullReferenceException("No IAvalonPacketClientReceiver found");
            }
            return _packetReceiver;
        }
    }

    private void OnEnable()
    {
        PacketReceiver.OnPacketReceived += Handle;
    }

    private void OnDisable()
    {
        PacketReceiver.OnPacketReceived -= Handle;
    }

    public static ClientPacketHandler Handler => _packetHandler;

    public void Send(AvalonPacket packet)
    {
        if(packet != null)
        {
            PacketSender?.Send(packet);
        //Debug.Log($"Client sending packet of type {packet.GetType().Name}");
        }
    }

    public void Handle(AvalonPacket packet)
    {
        //Debug.Log($"Client handling packet of type {packet.GetType().Name}");
        _packetHandler?.Handle(packet);
    }
}
