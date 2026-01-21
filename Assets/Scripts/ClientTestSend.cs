using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClientTestSend : MonoBehaviour
{
    private IAvalonClientPacketSender _packetSender;
    [SerializeField] private GameObject _observerPrefab;
    [SerializeField] private GameObject _localClientPrefab;
    private GameObject clientInstance = null;

    void Start()
    {
        _packetSender = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, sortMode: FindObjectsSortMode.None)
            .OfType<IAvalonClientPacketSender>()
            .FirstOrDefault();
    }

    private ClientPacketHandler _clientPacketHandler;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if(clientInstance != null)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                ConnectPlayerPacket packet = new ConnectPlayerPacket()
                {
                    PlayerId = LocalServerPacketSender.CurrentPlayerId,
                    PlayerName = "Test Player",
                    AuthToken = "abc123"
                };
                _packetSender.Send(packet);
                clientInstance = Instantiate(_localClientPrefab);
            }
            if (Input.GetKeyDown(KeyCode.O) && string.IsNullOrWhiteSpace(LocalServerPacketSender.CurrentObserverId))
            {
                if (clientInstance != null)
                {
                    return;
                }
                if (_clientPacketHandler == null)
                {
                    _clientPacketHandler = FindAnyObjectByType<ClientPacketHandler>();
                }
                _clientPacketHandler.RegisterClientHandler<ConnectObserverPacket>(ObserverConnectionCallback);
                ConnectObserverPacket connectPacket = new ConnectObserverPacket();
                _packetSender.Send(connectPacket);
            }
        }
    }

    private void ObserverConnectionCallback(ConnectObserverPacket packet)
    {
        var observeMapPacket = new ObserveMapPacket()
        {
            MapId = Constants.InitialCombatMap
        };
        observeMapPacket.SetClientId(packet.ObserverId);
        LocalServerPacketSender.CurrentObserverId = packet.ObserverId;
        _packetSender.Send(observeMapPacket);
        if (_clientPacketHandler == null)
        {
            _clientPacketHandler = FindAnyObjectByType<ClientPacketHandler>();
        }
        _clientPacketHandler.UnregisterClientHandler<ConnectObserverPacket>();
        clientInstance = Instantiate(_observerPrefab);
    }
}
