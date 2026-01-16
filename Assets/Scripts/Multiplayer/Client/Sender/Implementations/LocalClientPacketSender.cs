using UnityEngine;

public class LocalClientPacketSender : MonoBehaviour, IAvalonClientPacketSender
{
    [SerializeField] private LocalServerPacketReceiver _localPacketReceiver;

    private void Awake()
    {
        _localPacketReceiver = FindFirstObjectByType<LocalServerPacketReceiver>() ?? throw new System.NullReferenceException("No IAvalonClientPacketSender found");
    }

    public void Send(AvalonPacket packet)
    {
        _localPacketReceiver.Handle(packet);
    }
}
