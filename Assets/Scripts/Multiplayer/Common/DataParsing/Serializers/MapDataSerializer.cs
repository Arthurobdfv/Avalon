using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializer for SpawnPointData.
/// Note: Only serializes the data, not the ScriptableObject reference.
/// </summary>
public class SpawnPointDataSerializer : ITypeSerializer<SpawnPointData>
{
    private static readonly ListSerializer<EnemyDataEnum> _entityIdListSerializer =
        new ListSerializer<EnemyDataEnum>(TypeSerializers.Enum<EnemyDataEnum>());

    public int FixedSize => -1; // Variable due to list

    public int Serialize(SpawnPointData value, byte[] buffer, int offset)
    {
        int startOffset = offset;

        if (value == null)
        {
            TypeSerializers.Bool.Serialize(false, buffer, offset);
            return 1;
        }

        // Null marker (exists = true)
        offset += TypeSerializers.Bool.Serialize(true, buffer, offset);

        // RespawnTime (float)
        offset += TypeSerializers.Float.Serialize(value.RespawnTime, buffer, offset);

        // SpawnTransform (Vector2)
        offset += TypeSerializers.Vector2.Serialize(value.SpawnTransform, buffer, offset);

        // EntityIDs (List<EnemyDataEnum>)
        offset += _entityIdListSerializer.Serialize(value.EntityIDs, buffer, offset);

        return offset - startOffset;
    }

    public int Deserialize(byte[] buffer, int offset, out SpawnPointData value)
    {
        int startOffset = offset;

        offset += TypeSerializers.Bool.Deserialize(buffer, offset, out bool exists);
        if (!exists)
        {
            value = null;
            return offset - startOffset;
        }

        // Create a runtime instance (not a ScriptableObject asset)
        value = ScriptableObject.CreateInstance<SpawnPointData>();

        offset += TypeSerializers.Float.Deserialize(buffer, offset, out float respawnTime);
        offset += TypeSerializers.Vector2.Deserialize(buffer, offset, out Vector2 spawnTransform);
        offset += _entityIdListSerializer.Deserialize(buffer, offset, out List<EnemyDataEnum> entityIds);

        // Note: ScriptableObjects have private setters - you may need to:
        // 1. Add internal/public setters, or
        // 2. Use reflection, or
        // 3. Create a DTO (Data Transfer Object) for network transfer

        return offset - startOffset;
    }
}

/// <summary>
/// Serializer for WarpPoint.
/// </summary>
public class WarpPointSerializer : ITypeSerializer<WarpPoint>
{
    public int FixedSize => -1; // Variable due to strings

    public int Serialize(WarpPoint value, byte[] buffer, int offset)
    {
        int startOffset = offset;

        if (value == null)
        {
            offset += TypeSerializers.Bool.Serialize(false, buffer, offset);
            return offset - startOffset;
        }

        offset += TypeSerializers.Bool.Serialize(true, buffer, offset);
        offset += TypeSerializers.String.Serialize(value.warpID, buffer, offset);
        offset += TypeSerializers.Vector2.Serialize(value.warpPosition, buffer, offset);

        // For destinationMap, we only send the MapID (reference by ID)
        offset += TypeSerializers.String.Serialize(value.destinationMap?.MapID, buffer, offset);

        return offset - startOffset;
    }

    public int Deserialize(byte[] buffer, int offset, out WarpPoint value)
    {
        int startOffset = offset;

        offset += TypeSerializers.Bool.Deserialize(buffer, offset, out bool exists);
        if (!exists)
        {
            value = null;
            return offset - startOffset;
        }

        value = ScriptableObject.CreateInstance<WarpPoint>();

        offset += TypeSerializers.String.Deserialize(buffer, offset, out string warpId);
        offset += TypeSerializers.Vector2.Deserialize(buffer, offset, out Vector2 warpPosition);
        offset += TypeSerializers.String.Deserialize(buffer, offset, out string destMapId);

        // Note: Need to resolve destMapId to actual MapData asset on client side
        // This serializer only transfers the data, not asset references

        return offset - startOffset;
    }
}

/// <summary>
/// Serializer for MapData.
/// Transfers map data over the network - not the Unity asset reference.
/// </summary>
public class MapDataSerializer : ITypeSerializer<MapData>
{
    private static readonly ListSerializer<SpawnPointData> _spawnPointsSerializer =
        new ListSerializer<SpawnPointData>(new SpawnPointDataSerializer());
    private static readonly ListSerializer<WarpPoint> _warpPointsSerializer =
        new ListSerializer<WarpPoint>(new WarpPointSerializer());

    public int FixedSize => -1;

    public int Serialize(MapData value, byte[] buffer, int offset)
    {
        int startOffset = offset;

        if (value == null)
        {
            offset += TypeSerializers.Bool.Serialize(false, buffer, offset);
            return offset - startOffset;
        }

        offset += TypeSerializers.Bool.Serialize(true, buffer, offset);
        offset += TypeSerializers.String.Serialize(value.MapID, buffer, offset);
        offset += _spawnPointsSerializer.Serialize(value.SpawnPoints, buffer, offset);
        offset += _warpPointsSerializer.Serialize(value.WarpPoints, buffer, offset);

        return offset - startOffset;
    }

    public int Deserialize(byte[] buffer, int offset, out MapData value)
    {
        int startOffset = offset;

        offset += TypeSerializers.Bool.Deserialize(buffer, offset, out bool exists);
        if (!exists)
        {
            value = null;
            return offset - startOffset;
        }

        value = ScriptableObject.CreateInstance<MapData>();

        offset += TypeSerializers.String.Deserialize(buffer, offset, out string mapId);
        offset += _spawnPointsSerializer.Deserialize(buffer, offset, out List<SpawnPointData> spawnPoints);
        offset += _warpPointsSerializer.Deserialize(buffer, offset, out List<WarpPoint> warpPoints);

        // Note: Deserialized values need to be assigned via reflection or modified setters

        return offset - startOffset;
    }
}

/// <summary>
/// Static access to map-related serializers.
/// </summary>
public static class MapSerializers
{
    public static readonly ITypeSerializer<MapData> MapData = new MapDataSerializer();
    public static readonly ITypeSerializer<SpawnPointData> SpawnPointData = new SpawnPointDataSerializer();
    public static readonly ITypeSerializer<WarpPoint> WarpPoint = new WarpPointSerializer();
}
