/// <summary>
/// Interface for serializing/deserializing a specific type to/from bytes.
/// Implement this for each type you need to serialize.
/// </summary>
public interface ITypeSerializer<T>
{
    /// <summary>
    /// Returns the fixed size in bytes, or -1 if variable-length.
    /// </summary>
    int FixedSize { get; }

    /// <summary>
    /// Serializes the value to the buffer at the given offset.
    /// Returns the number of bytes written.
    /// </summary>
    int Serialize(T value, byte[] buffer, int offset);

    /// <summary>
    /// Deserializes a value from the buffer at the given offset.
    /// Returns the number of bytes read.
    /// </summary>
    int Deserialize(byte[] buffer, int offset, out T value);
}
