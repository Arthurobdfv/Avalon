using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerConnectionManager : MonoBehaviour
{
    private ServerCommunicationLayerManager _serverCommunicationManager;
    public GlobalEntitiesManager GlobalEntitiesManager;
    private Dictionary<string, ObserverData> Observers = new();
    private void Start()
    {
        if (_serverCommunicationManager == null)
        {
            _serverCommunicationManager = FindObjectOfType<ServerCommunicationLayerManager>();
        }
        _serverCommunicationManager.Handler.RegisterServerHandler<ConnectPlayerPacket>(OnConnectPlayer);
        _serverCommunicationManager.Handler.RegisterServerHandler<ConnectObserverPacket>(OnObserverConnect);
        _serverCommunicationManager.Handler.RegisterServerHandler<ObserveMapPacket>(ObserveMap);
    }

    private void ObserveMap(ObserveMapPacket packet)
    {
        if (!Observers.ContainsKey(packet.ClientId))
        {
            Debug.LogWarning($"Observer with ID {packet.ClientId} is not connected.");
            return;
        }
        var observerData = Observers[packet.ClientId] as MapObserverData;
        if (observerData != null)
        {
            Debug.Log($"Observer {packet.ClientId} is now observing map {packet.MapId}.");
            observerData.MapId = packet.MapId;
        }
        else if (observerData == null && Observers[packet.ClientId] is ObserverData)
        {
            observerData = new MapObserverData { ObserverId = packet.ClientId };
            Observers[packet.ClientId] = observerData;
            observerData.MapId = packet.MapId;
            _serverCommunicationManager.RegisterObserver<EntitySpawnPacket>(packet.ClientId, (entitySpawnPacket) =>
            {
                return observerData.MapId == entitySpawnPacket.MapId;
            });
        }
    }

    private void OnObserverConnect(ConnectObserverPacket packet)
    {
        if (string.IsNullOrEmpty(packet.ObserverId))
        {
            var observerId = $"obs_{Guid.NewGuid().ToString()}";
            packet.SetClientId(observerId);
            packet.ObserverId = observerId; 
        }
        if (Observers.ContainsKey(packet.ObserverId))
        {
            Debug.LogWarning($"Observer with ID {packet.ObserverId} is already connected.");
            return;
        }
        var observerData = new ObserverData { ObserverId = packet.ObserverId };
        Observers.Add(packet.ObserverId, observerData);
        var response = new ConnectObserverPacket()
        {
            ObserverId = observerData.ObserverId,
        };
        response.SetClientId(packet.ObserverId);
        _serverCommunicationManager.Send(response);
    }

    private void OnDestroy()
    {
        _serverCommunicationManager.Handler.UnregisterServerHandler<ConnectPlayerPacket>();
        _serverCommunicationManager.Handler.UnregisterServerHandler<ConnectObserverPacket>();
        _serverCommunicationManager.Handler.UnregisterServerHandler<ObserveMapPacket>();
    }

    private void OnConnectPlayer(ConnectPlayerPacket packet)
    {
        Debug.Log($"Player connected: {packet.PlayerName}");
        GlobalEntitiesManager.OnPlayerConnect(packet);
        var playerStartEquip = new PlayerEquipmentPacket()
        {
            PlayerBaseAsset = PlayerBaseAssetEnum.PLAYER_FEMALE_BASE_01
        };
        playerStartEquip.SetClientId(packet.PlayerId);
        _serverCommunicationManager.Send(playerStartEquip);
    }

    private class ObserverData
    {
        public string ObserverId { get; set; }
    }

    private class MapObserverData : ObserverData
    {
        public string MapId { get; set; }
    }
}
