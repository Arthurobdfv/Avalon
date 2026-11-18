using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resolves ITypeSerializer instances based on property type.
/// Automatically infers the correct serializer for common types.
/// </summary>
public static class TypeSerializerResolver
{
    private static readonly Dictionary<Type, object> _serializers = new Dictionary<Type, object>();

    static TypeSerializerResolver()
    {
        // Register built-in serializers
        Register(TypeSerializers.Int32);
        Register(TypeSerializers.Float);
        Register(TypeSerializers.Bool);
        Register(TypeSerializers.String);
        Register(TypeSerializers.Vector2);
        Register(TypeSerializers.Vector3);
    }

    /// <summary>
    /// Registers a serializer for a specific type.
    /// </summary>
    public static void Register<T>(ITypeSerializer<T> serializer)
    {
        _serializers[typeof(T)] = serializer;
    }

    /// <summary>
    /// Gets the serializer for a type. Automatically handles enums.
    /// </summary>
    public static ITypeSerializer<T> Get<T>()
    {
        var type = typeof(T);

        // Check if already registered
        if (_serializers.TryGetValue(type, out var serializer))
        {
            return (ITypeSerializer<T>)serializer;
        }

        // Handle enums automatically
        if (type.IsEnum)
        {
            var enumSerializer = CreateEnumSerializer<T>();
            _serializers[type] = enumSerializer;
            return enumSerializer;
        }

        throw new InvalidOperationException(
            $"No serializer registered for type {type.Name}. " +
            $"Register one using TypeSerializerResolver.Register<{type.Name}>()");
    }

    /// <summary>
    /// Tries to get a serializer for a type.
    /// </summary>
    public static bool TryGet<T>(out ITypeSerializer<T> serializer)
    {
        var type = typeof(T);

        if (_serializers.TryGetValue(type, out var obj))
        {
            serializer = (ITypeSerializer<T>)obj;
            return true;
        }

        if (type.IsEnum)
        {
            serializer = CreateEnumSerializer<T>();
            _serializers[type] = serializer;
            return true;
        }

        serializer = null;
        return false;
    }

    private static ITypeSerializer<T> CreateEnumSerializer<T>()
    {
        // Create EnumSerializer<T> dynamically
        var enumSerializerType = typeof(EnumSerializer<>).MakeGenericType(typeof(T));
        return (ITypeSerializer<T>)Activator.CreateInstance(enumSerializerType);
    }
}
