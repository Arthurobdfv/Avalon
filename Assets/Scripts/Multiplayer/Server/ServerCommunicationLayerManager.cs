using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ServerCommunicationLayerManager : MonoBehaviour
{
    IAvalonPacketServerSender _packetSender;
    IAvalonPacketServerReceiver _packetReceiver;

    readonly ServerPacketHandler _packetHandler = new();

    public ServerPacketHandler Handler => _packetHandler;

    private void Awake()
    {
        _packetSender = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IAvalonPacketServerSender>().FirstOrDefault() ?? throw new NullReferenceException("No IAvalonPacketServerSender found");
        // Change this to service provider fetching since network sender might not be monobehaviour in future
        _packetReceiver = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IAvalonPacketServerReceiver>().FirstOrDefault() ?? throw new NullReferenceException("No IAvalonPacketServerReceiver found");
    }

    private void OnEnable()
    {
        _packetReceiver.OnPacketReceived += Handle;
    }

    private void OnDisable()
    {
        _packetReceiver.OnPacketReceived -= Handle;
    }

    internal void Handle(AvalonPacket packet)
    {
        //Debug.Log($"[ServerCommunicationLayerManager] Handling packet of type {packet.GetType().Name}");
        _packetHandler.Handle(packet);
    }

    internal void Send(AvalonPacket packet)
    {
        //Debug.Log($"[ServerCommunicationLayerManager] Sending packet of type {packet.GetType().Name}");
        _packetSender.Send(packet);
    }

    internal void SendMap(AvalonPacket packet, string mapId)
    {
        _packetSender.Send(packet, MapManager.GetEntityByType(EntityTypeEnum.Player, mapId).Concat(GetPacketObservers(packet)));
    }

    internal IEnumerable<string> GetPacketObservers(AvalonPacket packet)
    {
        var packetType = packet.GetType();
        if (_packetObserversCache.ContainsKey(packetType))
        {
            return _packetObserversCache[packetType].Where(x => x.Predicate.Invoke(packet)).Select(x => x.ObserverId);
        }
        else return new List<string>();
    }

    public void RegisterObserver<T>(string observerId, Predicate<T> predicate) where T : AvalonPacket
    {
        var packetType = typeof(T);
        if (!_packetObserversCache.ContainsKey(packetType))
        {
            _packetObserversCache[packetType] = new List<ObserverInfo>();
        }
        _packetObserversCache[packetType].Add(new ObserverInfo
        {
            ObserverId = observerId,
            Predicate = (AvalonPacket packet) => predicate((T)packet)
        });
    }

    public void UnregisterObserver<T>(string observerId) where T : AvalonPacket
    {
        var packetType = typeof(T);
        if (_packetObserversCache.ContainsKey(packetType))
        {
            _packetObserversCache[packetType].RemoveAll(x => x.ObserverId == observerId);
        }
    }

    private readonly Dictionary<Type, List<ObserverInfo>> _packetObserversCache = new();

    private class ObserverInfo
    {
        public string ObserverId;
        public Predicate<AvalonPacket> Predicate;
    }
}
