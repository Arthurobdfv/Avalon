using System;

/// <summary>
/// Describes a single property for serialization/deserialization.
/// The order in which descriptors are added to a PacketDescriber determines
/// the order they are written to and read from the byte array.
/// </summary>
public abstract class PropertyDescriptor
{
    /// <summary>
    /// Writes this property's value to the buffer at the given offset.
    /// Returns the number of bytes written.
    /// </summary>
    public abstract int Serialize(object instance, byte[] buffer, int offset);

    /// <summary>
    /// Reads this property's value from the buffer at the given offset.
    /// Returns the number of bytes read.
    /// </summary>
    public abstract int Deserialize(object instance, byte[] buffer, int offset);

    /// <summary>
    /// Gets the size in bytes this property will occupy.
    /// For variable-length types, returns -1.
    /// </summary>
    public abstract int GetFixedSize();
}

/// <summary>
/// Generic property descriptor with getter/setter accessors.
/// </summary>
public class PropertyDescriptor<TPacket, TProperty> : PropertyDescriptor
{
    private readonly Func<TPacket, TProperty> _getter;
    private readonly Action<TPacket, TProperty> _setter;
    private readonly ITypeSerializer<TProperty> _serializer;

    public PropertyDescriptor(
        Func<TPacket, TProperty> getter,
        Action<TPacket, TProperty> setter,
        ITypeSerializer<TProperty> serializer)
    {
        _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        _setter = setter ?? throw new ArgumentNullException(nameof(setter));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public override int Serialize(object instance, byte[] buffer, int offset)
    {
        var value = _getter((TPacket)instance);
        return _serializer.Serialize(value, buffer, offset);
    }

    public override int Deserialize(object instance, byte[] buffer, int offset)
    {
        int bytesRead = _serializer.Deserialize(buffer, offset, out TProperty value);
        _setter((TPacket)instance, value);
        return bytesRead;
    }

    public override int GetFixedSize()
    {
        return _serializer.FixedSize;
    }
}
