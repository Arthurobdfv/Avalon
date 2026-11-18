using System;
using System.Collections.Generic;

/// <summary>
/// Central registry for packet describers.
/// Register all packet types here to enable type-safe serialization/deserialization.
/// 
/// Packet wire format:
/// [4 bytes: Packet Type ID][N bytes: ClientId (length-prefixed string)][M bytes: Payload]
/// </summary>
public static class PacketDescriberRegistry
{
    private static readonly Dictionary<Type, IPacketDescriber> _describers = new Dictionary<Type, IPacketDescriber>();
    private static readonly Dictionary<int, Type> _packetTypeIdToType = new Dictionary<int, Type>();
    private static readonly Dictionary<Type, int> _typeToPacketTypeId = new Dictionary<Type, int>();
    private static readonly StringSerializer _stringSerializer = new StringSerializer();
    private static bool _initialized;

    /// <summary>
    /// Registers a packet describer for a specific packet type.
    /// </summary>
    public static void Register<TPacket>(int packetTypeId, PacketDescriber<TPacket> describer)
        where TPacket : AvalonPacket, new()
    {
        var type = typeof(TPacket);
        _describers[type] = describer;
        _packetTypeIdToType[packetTypeId] = type;
        _typeToPacketTypeId[type] = packetTypeId;
    }

    /// <summary>
    /// Gets the describer for a packet type.
    /// </summary>
    public static PacketDescriber<TPacket> Get<TPacket>() where TPacket : AvalonPacket, new()
    {
        EnsureInitialized();
        if (_describers.TryGetValue(typeof(TPacket), out var describer))
        {
            return (PacketDescriber<TPacket>)describer;
        }
        throw new InvalidOperationException($"No describer registered for {typeof(TPacket).Name}");
    }

    /// <summary>
    /// Gets the packet type ID for a packet type.
    /// </summary>
    public static int GetPacketTypeId<TPacket>() where TPacket : AvalonPacket
    {
        EnsureInitialized();
        if (_typeToPacketTypeId.TryGetValue(typeof(TPacket), out int id))
        {
            return id;
        }
        throw new InvalidOperationException($"No packet type ID registered for {typeof(TPacket).Name}");
    }

    /// <summary>
    /// Gets the packet type ID for a packet instance.
    /// </summary>
    public static int GetPacketTypeId(AvalonPacket packet)
    {
        EnsureInitialized();
        if (_typeToPacketTypeId.TryGetValue(packet.GetType(), out int id))
        {
            return id;
        }
        throw new InvalidOperationException($"No packet type ID registered for {packet.GetType().Name}");
    }

    /// <summary>
    /// Gets the packet type for a packet type ID.
    /// </summary>
    public static Type GetPacketType(int packetTypeId)
    {
        EnsureInitialized();
        if (_packetTypeIdToType.TryGetValue(packetTypeId, out var type))
        {
            return type;
        }
        throw new InvalidOperationException($"No packet type registered for ID {packetTypeId}");
    }

    /// <summary>
    /// Serializes any registered packet to bytes.
    /// Wire format: [4 bytes: PacketTypeId][ClientId][Payload]
    /// No reflection used - uses IPacketDescriber interface.
    /// </summary>
    public static byte[] Serialize(AvalonPacket packet)
    {
        EnsureInitialized();
        var type = packet.GetType();

        if (!_typeToPacketTypeId.TryGetValue(type, out int packetTypeId))
        {
            throw new InvalidOperationException($"No describer registered for {type.Name}");
        }

        if (!_describers.TryGetValue(type, out var describer))
        {
            throw new InvalidOperationException($"No describer registered for {type.Name}");
        }

        // Serialize type-specific payload (no reflection - uses interface)
        byte[] payload = describer.SerializePacket(packet);

        // Calculate ClientId size
        byte[] clientIdBuffer = new byte[256]; // Max ClientId size
        int clientIdSize = _stringSerializer.Serialize(packet.ClientId, clientIdBuffer, 0);

        // Build final buffer: [PacketTypeId (4)][ClientId (variable)][Payload]
        byte[] result = new byte[4 + clientIdSize + payload.Length];
        int offset = 0;

        // Write packet type ID (4 bytes, big-endian)
        result[offset++] = (byte)(packetTypeId >> 24);
        result[offset++] = (byte)(packetTypeId >> 16);
        result[offset++] = (byte)(packetTypeId >> 8);
        result[offset++] = (byte)packetTypeId;

        // Write ClientId
        Array.Copy(clientIdBuffer, 0, result, offset, clientIdSize);
        offset += clientIdSize;

        // Write payload
        Array.Copy(payload, 0, result, offset, payload.Length);

        return result;
    }

    /// <summary>
    /// Deserializes bytes to a packet.
    /// Expects wire format: [4 bytes: PacketTypeId][ClientId][Payload]
    /// </summary>
    public static AvalonPacket Deserialize(byte[] buffer)
    {
        return Deserialize(buffer, 0, out _);
    }

    /// <summary>
    /// Deserializes bytes to a packet at the given offset.
    /// No reflection used - uses IPacketDescriber interface.
    /// </summary>
    public static AvalonPacket Deserialize(byte[] buffer, int offset, out int bytesRead)
    {
        EnsureInitialized();
        int startOffset = offset;

        // Read packet type ID (4 bytes, big-endian)
        int packetTypeId = (buffer[offset] << 24) |
                           (buffer[offset + 1] << 16) |
                           (buffer[offset + 2] << 8) |
                           buffer[offset + 3];
        offset += 4;

        if (!_packetTypeIdToType.TryGetValue(packetTypeId, out var type))
        {
            throw new InvalidOperationException($"No packet type registered for ID {packetTypeId}");
        }

        if (!_describers.TryGetValue(type, out var describer))
        {
            throw new InvalidOperationException($"No describer registered for {type.Name}");
        }

        // Read ClientId
        int clientIdBytesRead = _stringSerializer.Deserialize(buffer, offset, out string clientId);
        offset += clientIdBytesRead;

        // Deserialize payload (no reflection - uses interface)
        int payloadBytesRead;
        AvalonPacket packet = describer.DeserializePacket(buffer, offset, out payloadBytesRead);
        offset += payloadBytesRead;

        // Set ClientId on the packet
        packet.SetClientId(clientId);

        bytesRead = offset - startOffset;
        return packet;
    }

    private static void EnsureInitialized()
    {
        if (!_initialized)
        {
            PacketDescribers.RegisterAll();
            _initialized = true;
        }
    }
}
