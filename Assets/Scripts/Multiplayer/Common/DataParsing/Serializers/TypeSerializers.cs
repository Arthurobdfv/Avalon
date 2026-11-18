using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Built-in serializers for common types.
/// </summary>
public static class TypeSerializers
{
    public static readonly ITypeSerializer<int> Int32 = new Int32Serializer();
    public static readonly ITypeSerializer<float> Float = new FloatSerializer();
    public static readonly ITypeSerializer<bool> Bool = new BoolSerializer();
    public static readonly ITypeSerializer<string> String = new StringSerializer();
    public static readonly ITypeSerializer<Vector2> Vector2 = new Vector2Serializer();
    public static readonly ITypeSerializer<Vector3> Vector3 = new Vector3Serializer();

    /// <summary>
    /// Creates a serializer for any enum type backed by int.
    /// </summary>
    public static ITypeSerializer<TEnum> Enum<TEnum>() where TEnum : struct, Enum
    {
        return new EnumSerializer<TEnum>();
    }
}

public class Int32Serializer : ITypeSerializer<int>
{
    public int FixedSize => 4;

    public int Serialize(int value, byte[] buffer, int offset)
    {
        buffer[offset] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)value;
        return 4;
    }

    public int Deserialize(byte[] buffer, int offset, out int value)
    {
        value = (buffer[offset] << 24) |
                (buffer[offset + 1] << 16) |
                (buffer[offset + 2] << 8) |
                buffer[offset + 3];
        return 4;
    }
}

public class FloatSerializer : ITypeSerializer<float>
{
    public int FixedSize => 4;

    public int Serialize(float value, byte[] buffer, int offset)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        Array.Copy(bytes, 0, buffer, offset, 4);
        return 4;
    }

    public int Deserialize(byte[] buffer, int offset, out float value)
    {
        byte[] bytes = new byte[4];
        Array.Copy(buffer, offset, bytes, 0, 4);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        value = BitConverter.ToSingle(bytes, 0);
        return 4;
    }
}

public class BoolSerializer : ITypeSerializer<bool>
{
    public int FixedSize => 1;

    public int Serialize(bool value, byte[] buffer, int offset)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
        return 1;
    }

    public int Deserialize(byte[] buffer, int offset, out bool value)
    {
        value = buffer[offset] != 0;
        return 1;
    }
}

public class StringSerializer : ITypeSerializer<string>
{
    public int FixedSize => -1; // Variable length

    public int Serialize(string value, byte[] buffer, int offset)
    {
        if (value == null)
        {
            // Write length of -1 for null
            buffer[offset] = 0xFF;
            buffer[offset + 1] = 0xFF;
            buffer[offset + 2] = 0xFF;
            buffer[offset + 3] = 0xFF;
            return 4;
        }

        byte[] stringBytes = Encoding.UTF8.GetBytes(value);
        int length = stringBytes.Length;

        // Write length prefix (4 bytes)
        buffer[offset] = (byte)(length >> 24);
        buffer[offset + 1] = (byte)(length >> 16);
        buffer[offset + 2] = (byte)(length >> 8);
        buffer[offset + 3] = (byte)length;

        // Write string bytes
        Array.Copy(stringBytes, 0, buffer, offset + 4, length);

        return 4 + length;
    }

    public int Deserialize(byte[] buffer, int offset, out string value)
    {
        int length = (buffer[offset] << 24) |
                     (buffer[offset + 1] << 16) |
                     (buffer[offset + 2] << 8) |
                     buffer[offset + 3];

        if (length == -1)
        {
            value = null;
            return 4;
        }

        value = Encoding.UTF8.GetString(buffer, offset + 4, length);
        return 4 + length;
    }
}

public class Vector2Serializer : ITypeSerializer<Vector2>
{
    private static readonly FloatSerializer _floatSerializer = new FloatSerializer();

    public int FixedSize => 8; // 2 floats * 4 bytes

    public int Serialize(Vector2 value, byte[] buffer, int offset)
    {
        int written = 0;
        written += _floatSerializer.Serialize(value.x, buffer, offset + written);
        written += _floatSerializer.Serialize(value.y, buffer, offset + written);
        return written;
    }

    public int Deserialize(byte[] buffer, int offset, out Vector2 value)
    {
        int read = 0;
        read += _floatSerializer.Deserialize(buffer, offset + read, out float x);
        read += _floatSerializer.Deserialize(buffer, offset + read, out float y);
        value = new Vector2(x, y);
        return read;
    }
}

public class Vector3Serializer : ITypeSerializer<Vector3>
{
    private static readonly FloatSerializer _floatSerializer = new FloatSerializer();

    public int FixedSize => 12; // 3 floats * 4 bytes

    public int Serialize(Vector3 value, byte[] buffer, int offset)
    {
        int written = 0;
        written += _floatSerializer.Serialize(value.x, buffer, offset + written);
        written += _floatSerializer.Serialize(value.y, buffer, offset + written);
        written += _floatSerializer.Serialize(value.z, buffer, offset + written);
        return written;
    }

    public int Deserialize(byte[] buffer, int offset, out Vector3 value)
    {
        int read = 0;
        read += _floatSerializer.Deserialize(buffer, offset + read, out float x);
        read += _floatSerializer.Deserialize(buffer, offset + read, out float y);
        read += _floatSerializer.Deserialize(buffer, offset + read, out float z);
        value = new Vector3(x, y, z);
        return read;
    }
}

public class EnumSerializer<TEnum> : ITypeSerializer<TEnum> where TEnum : struct, Enum
{
    public int FixedSize => 4;

    public int Serialize(TEnum value, byte[] buffer, int offset)
    {
        int intValue = Convert.ToInt32(value);
        buffer[offset] = (byte)(intValue >> 24);
        buffer[offset + 1] = (byte)(intValue >> 16);
        buffer[offset + 2] = (byte)(intValue >> 8);
        buffer[offset + 3] = (byte)intValue;
        return 4;
    }

    public int Deserialize(byte[] buffer, int offset, out TEnum value)
    {
        int intValue = (buffer[offset] << 24) |
                       (buffer[offset + 1] << 16) |
                       (buffer[offset + 2] << 8) |
                       buffer[offset + 3];
        value = (TEnum)System.Enum.ToObject(typeof(TEnum), intValue);
        return 4;
    }
}
