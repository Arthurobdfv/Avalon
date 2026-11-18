using System;
using System.Collections.Generic;

/// <summary>
/// Non-generic interface for packet describers.
/// Allows type-erased serialization/deserialization without reflection.
/// </summary>
public interface IPacketDescriber
{
    /// <summary>
    /// Serializes a packet to a byte array (type-specific properties only, no header).
    /// </summary>
    byte[] SerializePacket(AvalonPacket packet);

    /// <summary>
    /// Serializes a packet into an existing buffer.
    /// Returns the number of bytes written.
    /// </summary>
    int SerializePacket(AvalonPacket packet, byte[] buffer, int offset);

    /// <summary>
    /// Deserializes a packet from a byte array at the specified offset.
    /// Returns the packet and the number of bytes read.
    /// </summary>
    AvalonPacket DeserializePacket(byte[] buffer, int offset, out int bytesRead);
}

/// <summary>
/// Describes the serialization layout of a packet type.
/// Properties are serialized/deserialized in the order they are added.
/// The serializer is automatically inferred from the property type.
/// 
/// Usage:
/// <code>
/// var describer = new PacketDescriber&lt;ConnectPlayerPacket&gt;()
///     .Property(p => p.PlayerId, (p, v) => p.PlayerId = v)
///     .Property(p => p.PlayerName, (p, v) => p.PlayerName = v)
///     .Property(p => p.AuthToken, (p, v) => p.AuthToken = v);
/// </code>
/// </summary>
public class PacketDescriber<TPacket> : IPacketDescriber where TPacket : AvalonPacket, new()
{
    private readonly List<PropertyDescriptor> _properties = new List<PropertyDescriptor>();

    /// <summary>
    /// Adds a property to the describer.
    /// The serializer is automatically inferred from TProperty.
    /// Properties are serialized in the order they are added.
    /// </summary>
    public PacketDescriber<TPacket> Property<TProperty>(
        Func<TPacket, TProperty> getter,
        Action<TPacket, TProperty> setter)
    {
        var serializer = TypeSerializerResolver.Get<TProperty>();
        _properties.Add(new PropertyDescriptor<TPacket, TProperty>(getter, setter, serializer));
        return this;
    }

    /// <summary>
    /// Adds a property with an explicit serializer (for custom/complex types).
    /// </summary>
    public PacketDescriber<TPacket> Property<TProperty>(
        Func<TPacket, TProperty> getter,
        Action<TPacket, TProperty> setter,
        ITypeSerializer<TProperty> serializer)
    {
        _properties.Add(new PropertyDescriptor<TPacket, TProperty>(getter, setter, serializer));
        return this;
    }

    /// <summary>
    /// Serializes the packet to a byte array.
    /// </summary>
    public byte[] Serialize(TPacket packet)
    {
        // First pass: calculate total size needed
        int totalSize = CalculateSerializedSize(packet);
        byte[] buffer = new byte[totalSize];

        // Second pass: write data
        int offset = 0;
        foreach (var prop in _properties)
        {
            offset += prop.Serialize(packet, buffer, offset);
        }

        return buffer;
    }

    /// <summary>
    /// Serializes the packet into an existing buffer.
    /// Returns the number of bytes written.
    /// </summary>
    public int Serialize(TPacket packet, byte[] buffer, int offset)
    {
        int startOffset = offset;
        foreach (var prop in _properties)
        {
            offset += prop.Serialize(packet, buffer, offset);
        }
        return offset - startOffset;
    }

    /// <summary>
    /// Deserializes a packet from a byte array.
    /// </summary>
    public TPacket Deserialize(byte[] buffer)
    {
        return Deserialize(buffer, 0, out _);
    }

    /// <summary>
    /// Deserializes a packet from a byte array at the specified offset.
    /// Returns the number of bytes read via bytesRead.
    /// </summary>
    public TPacket Deserialize(byte[] buffer, int offset, out int bytesRead)
    {
        TPacket packet = new TPacket();
        bytesRead = 0;

        foreach (var prop in _properties)
        {
            bytesRead += prop.Deserialize(packet, buffer, offset + bytesRead);
        }

        return packet;
    }

    /// <summary>
    /// Populates an existing packet instance from a byte array.
    /// </summary>
    public int DeserializeInto(TPacket packet, byte[] buffer, int offset)
    {
        int bytesRead = 0;
        foreach (var prop in _properties)
        {
            bytesRead += prop.Deserialize(packet, buffer, offset + bytesRead);
        }
        return bytesRead;
    }

    /// <summary>
    /// Gets the list of property descriptors for inspection.
    /// </summary>
    public IReadOnlyList<PropertyDescriptor> Properties => _properties;

    // IPacketDescriber implementation (type-erased, no reflection needed)
    byte[] IPacketDescriber.SerializePacket(AvalonPacket packet) => Serialize((TPacket)packet);
    int IPacketDescriber.SerializePacket(AvalonPacket packet, byte[] buffer, int offset) => Serialize((TPacket)packet, buffer, offset);
    AvalonPacket IPacketDescriber.DeserializePacket(byte[] buffer, int offset, out int bytesRead) => Deserialize(buffer, offset, out bytesRead);

    private int CalculateSerializedSize(TPacket packet)
    {
        // For now, use a large buffer and measure
        // A more efficient approach would be to have serializers report size before writing
        byte[] tempBuffer = new byte[64 * 1024]; // 64KB temp buffer
        int size = 0;
        foreach (var prop in _properties)
        {
            size += prop.Serialize(packet, tempBuffer, size);
        }
        return size;
    }
}
