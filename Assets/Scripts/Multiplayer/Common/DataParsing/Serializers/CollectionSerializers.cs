using System;
using System.Collections.Generic;

/// <summary>
/// Serializer for Dictionary types with string keys.
/// </summary>
public class DictionarySerializer<TValue> : ITypeSerializer<Dictionary<string, TValue>>
{
    private readonly ITypeSerializer<TValue> _valueSerializer;
    private readonly StringSerializer _stringSerializer = new StringSerializer();
    private readonly Int32Serializer _intSerializer = new Int32Serializer();

    public DictionarySerializer(ITypeSerializer<TValue> valueSerializer)
    {
        _valueSerializer = valueSerializer ?? throw new ArgumentNullException(nameof(valueSerializer));
    }

    public int FixedSize => -1; // Variable length

    public int Serialize(Dictionary<string, TValue> value, byte[] buffer, int offset)
    {
        int startOffset = offset;

        if (value == null)
        {
            offset += _intSerializer.Serialize(-1, buffer, offset);
            return offset - startOffset;
        }

        // Write count
        offset += _intSerializer.Serialize(value.Count, buffer, offset);

        // Write each key-value pair
        foreach (var kvp in value)
        {
            offset += _stringSerializer.Serialize(kvp.Key, buffer, offset);
            offset += _valueSerializer.Serialize(kvp.Value, buffer, offset);
        }

        return offset - startOffset;
    }

    public int Deserialize(byte[] buffer, int offset, out Dictionary<string, TValue> value)
    {
        int startOffset = offset;

        offset += _intSerializer.Deserialize(buffer, offset, out int count);

        if (count == -1)
        {
            value = null;
            return offset - startOffset;
        }

        value = new Dictionary<string, TValue>(count);

        for (int i = 0; i < count; i++)
        {
            offset += _stringSerializer.Deserialize(buffer, offset, out string key);
            offset += _valueSerializer.Deserialize(buffer, offset, out TValue itemValue);
            value[key] = itemValue;
        }

        return offset - startOffset;
    }
}

/// <summary>
/// Serializer for List types.
/// </summary>
public class ListSerializer<T> : ITypeSerializer<List<T>>
{
    private readonly ITypeSerializer<T> _itemSerializer;
    private readonly Int32Serializer _intSerializer = new Int32Serializer();

    public ListSerializer(ITypeSerializer<T> itemSerializer)
    {
        _itemSerializer = itemSerializer ?? throw new ArgumentNullException(nameof(itemSerializer));
    }

    public int FixedSize => -1; // Variable length

    public int Serialize(List<T> value, byte[] buffer, int offset)
    {
        int startOffset = offset;

        if (value == null)
        {
            offset += _intSerializer.Serialize(-1, buffer, offset);
            return offset - startOffset;
        }

        // Write count
        offset += _intSerializer.Serialize(value.Count, buffer, offset);

        // Write each item
        foreach (var item in value)
        {
            offset += _itemSerializer.Serialize(item, buffer, offset);
        }

        return offset - startOffset;
    }

    public int Deserialize(byte[] buffer, int offset, out List<T> value)
    {
        int startOffset = offset;

        offset += _intSerializer.Deserialize(buffer, offset, out int count);

        if (count == -1)
        {
            value = null;
            return offset - startOffset;
        }

        value = new List<T>(count);

        for (int i = 0; i < count; i++)
        {
            offset += _itemSerializer.Deserialize(buffer, offset, out T item);
            value.Add(item);
        }

        return offset - startOffset;
    }
}
