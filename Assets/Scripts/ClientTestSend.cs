using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClientTestSend : MonoBehaviour
{
    [SerializeField] private GameObject _observerPrefab;
    [SerializeField] private GameObject _localClientPrefab;
    private ClientCommunicationLayerManager _clientCommunicationLayerManager;
    private GameObject clientInstance = null;

    private ClientCommunicationLayerManager ClientCommunicationManager
    {
        get
        {
            if (_clientCommunicationLayerManager == null)
            {
                _clientCommunicationLayerManager = FindAnyObjectByType<ClientCommunicationLayerManager>();
            }
            return _clientCommunicationLayerManager;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (clientInstance != null)
        {
            HandleClientCommands();
        }
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if (clientInstance != null)
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
                ClientCommunicationManager.Send(packet);
                clientInstance = Instantiate(_localClientPrefab);
            }
            if (Input.GetKeyDown(KeyCode.O) && string.IsNullOrWhiteSpace(LocalServerPacketSender.CurrentObserverId))
            {
                if (clientInstance != null)
                {
                    return;
                }
                ClientCommunicationManager.Handler.RegisterClientHandler<ConnectObserverPacket>(ObserverConnectionCallback);
                ConnectObserverPacket connectPacket = new ConnectObserverPacket();
                ClientCommunicationManager.Send(connectPacket);
            }
        }
    }

    private void HandleClientCommands()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var playerEquipmentPacket = new PlayerEquipmentPacket()
            {
                PlayerBaseAsset = PlayerBaseAssetEnum.PLAYER_ADVENTURER_BASE_01
            };
            ClientCommunicationManager.Send(playerEquipmentPacket);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            var playerEquipmentPacket = new PlayerEquipmentPacket()
            {
                PlayerBaseAsset = PlayerBaseAssetEnum.PLAYER_FEMALE_BASE_01
            };
            ClientCommunicationManager.Send(playerEquipmentPacket);
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
        ClientCommunicationManager.Send(observeMapPacket);
        ClientCommunicationManager.Handler.UnregisterClientHandler<ConnectObserverPacket>();
        clientInstance = Instantiate(_observerPrefab);
    }
}
