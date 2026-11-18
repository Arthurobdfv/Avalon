using UnityEngine;

/// <summary>
/// Defines packet describers for all Avalon packet types.
/// This is the central place to define the serialization layout for each packet.
/// 
/// Packet Type IDs:
/// 1-99: Connection/Observer packets
/// 100-199: Entity packets
/// 200-299: Player action packets
/// 300-399: Map packets
/// </summary>
public static class PacketDescribers
{
    // Connection/Observer packets (1-99)
    public const int ConnectPlayerPacketId = 1;
    public const int ConnectObserverPacketId = 2;
    public const int ObserveMapPacketId = 3;

    // Entity packets (100-199)
    public const int EntitySpawnPacketId = 100;
    public const int PlayerEquipmentPacketId = 101;

    // Player action packets (200-299)
    public const int PlayerInputStateId = 200;

    // Map packets (300-399)
    public const int MapDataSpawnPacketId = 300;

    /// <summary>
    /// Registers all packet describers with the registry.
    /// Called automatically on first use.
    /// </summary>
    public static void RegisterAll()
    {
        RegisterConnectionPackets();
        RegisterEntityPackets();
        RegisterPlayerActionPackets();
    }

    private static void RegisterConnectionPackets()
    {
        // ConnectPlayerPacket - serializer auto-inferred (string, string, string)
        var connectPlayerDescriber = new PacketDescriber<ConnectPlayerPacket>()
            .Property(p => p.PlayerId, (p, v) => p.PlayerId = v)
            .Property(p => p.PlayerName, (p, v) => p.PlayerName = v)
            .Property(p => p.AuthToken, (p, v) => p.AuthToken = v);
        PacketDescriberRegistry.Register(ConnectPlayerPacketId, connectPlayerDescriber);

        // ConnectObserverPacket
        var connectObserverDescriber = new PacketDescriber<ConnectObserverPacket>()
            .Property(p => p.ObserverId, (p, v) => p.ObserverId = v)
            .Property(p => p.ObserverName, (p, v) => p.ObserverName = v);
        PacketDescriberRegistry.Register(ConnectObserverPacketId, connectObserverDescriber);

        // ObserveMapPacket
        var observeMapDescriber = new PacketDescriber<ObserveMapPacket>()
            .Property(p => p.MapId, (p, v) => p.MapId = v);
        PacketDescriberRegistry.Register(ObserveMapPacketId, observeMapDescriber);
    }

    private static void RegisterEntityPackets()
    {
        // PlayerEquipmentPacket - enum auto-inferred
        var playerEquipmentDescriber = new PacketDescriber<PlayerEquipmentPacket>()
            .Property(p => p.PlayerId, (p, v) => p.PlayerId = v)
            .Property(p => p.PlayerBaseAsset, (p, v) => p.PlayerBaseAsset = v);
        PacketDescriberRegistry.Register(PlayerEquipmentPacketId, playerEquipmentDescriber);

        // EntitySpawnPacket - uses explicit serializer for complex Dictionary type
        var entitySpawnDescriber = new PacketDescriber<EntitySpawnPacket>()
            .Property(p => p.MapId, (p, v) => p.MapId = v)
            .Property(p => p.Entities, (p, v) => p.Entities = v, EntitySerializers.EntityDictionary());
        PacketDescriberRegistry.Register(EntitySpawnPacketId, entitySpawnDescriber);

        // MapDataSpawnPacket - uses explicit serializer for MapData
        var mapDataDescriber = new PacketDescriber<MapDataSpawnPacket>()
            .Property(p => p.MapData, (p, v) => p.MapData = v, MapSerializers.MapData);
        PacketDescriberRegistry.Register(MapDataSpawnPacketId, mapDataDescriber);
    }

    private static void RegisterPlayerActionPackets()
    {
        // PlayerInputState - player input from client to server
        var playerInputStateDescriber = new PacketDescriber<PlayerInputState>()
            .Property(p => p.LookDirection, (p, v) => p.LookDirection = v)
            .Property(p => p.MoveDirection, (p, v) => p.MoveDirection = v)
            .Property(p => p.AttackAction, (p, v) => p.AttackAction = v)
            .Property(p => p.SprintAction, (p, v) => p.SprintAction = v)
            .Property(p => p.InteractAction, (p, v) => p.InteractAction = v);
        PacketDescriberRegistry.Register(PlayerInputStateId, playerInputStateDescriber);
    }
}
