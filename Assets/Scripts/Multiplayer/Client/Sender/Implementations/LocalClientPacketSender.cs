using UnityEngine;

public class LocalClientPacketSender : MonoBehaviour, IAvalonClientPacketSender
{
    [SerializeField] private ServerCommunicationLayerManager _localPacketReceiver;
    private bool _isInitialized = false;

    ServerCommunicationLayerManager LocalPacketReceiver
    {
        get
        {
            if (_localPacketReceiver == null)
            {
                _localPacketReceiver = FindFirstObjectByType<ServerCommunicationLayerManager>() ?? throw new System.NullReferenceException("No IAvalonClientPacketSender found");
            }
            return _localPacketReceiver;
        }
    }

    public void Update()
    {
        if (_isInitialized)
        {
            ConnectPlayerPacket connectPacket = new ConnectPlayerPacket
            {
                PlayerName = "Admin"
            };
            connectPacket.PlayerId = System.Guid.NewGuid().ToString();
            Send(connectPacket);
            _isInitialized = true;
        }
    }

    public void Send(AvalonPacket packet)
    {
        if (string.IsNullOrEmpty(packet.ClientId))
        {
            packet.SetClientId(LocalServerPacketSender.CurrentPlayerId);
        }
        LocalPacketReceiver.Handle(packet);
    }
}
