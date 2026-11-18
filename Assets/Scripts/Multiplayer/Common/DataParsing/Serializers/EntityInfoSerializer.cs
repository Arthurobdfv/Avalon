using System;
using UnityEngine;

/// <summary>
/// Serializer for EntityInfo and its derived types.
/// Uses a type discriminator byte to handle polymorphism.
/// </summary>
public class EntityInfoSerializer : ITypeSerializer<EntityInfo>
{
    private const byte TypeIdBase = 0;
    private const byte TypeIdPlayer = 1;
    private const byte TypeIdEnemy = 2;

    public int FixedSize => -1; // Variable due to polymorphism

    public int Serialize(EntityInfo value, byte[] buffer, int offset)
    {
        int startOffset = offset;

        if (value == null)
        {
            buffer[offset] = 0xFF; // Null marker
            return 1;
        }

        // Write type discriminator
        if (value is PlayerEntityInfo playerInfo)
        {
            buffer[offset++] = TypeIdPlayer;
            offset += SerializeBase(value, buffer, offset);
            offset += SerializePlayerInfo(playerInfo, buffer, offset);
        }
        else if (value is EnemyEntityInfo enemyInfo)
        {
            buffer[offset++] = TypeIdEnemy;
            offset += SerializeBase(value, buffer, offset);
            offset += SerializeEnemyInfo(enemyInfo, buffer, offset);
        }
        else
        {
            buffer[offset++] = TypeIdBase;
            offset += SerializeBase(value, buffer, offset);
        }

        return offset - startOffset;
    }

    public int Deserialize(byte[] buffer, int offset, out EntityInfo value)
    {
        int startOffset = offset;

        byte typeId = buffer[offset++];

        if (typeId == 0xFF)
        {
            value = null;
            return 1;
        }

        switch (typeId)
        {
            case TypeIdPlayer:
            {
                var info = new PlayerEntityInfo();
                offset += DeserializeBase(info, buffer, offset);
                offset += DeserializePlayerInfo(info, buffer, offset);
                value = info;
                break;
            }
            case TypeIdEnemy:
            {
                var info = new EnemyEntityInfo();
                offset += DeserializeBase(info, buffer, offset);
                offset += DeserializeEnemyInfo(info, buffer, offset);
                value = info;
                break;
            }
            default:
            {
                var info = new EntityInfo();
                offset += DeserializeBase(info, buffer, offset);
                value = info;
                break;
            }
        }

        return offset - startOffset;
    }

    private int SerializeBase(EntityInfo info, byte[] buffer, int offset)
    {
        int startOffset = offset;

        // EntityType (enum as int)
        offset += TypeSerializers.Enum<EntityTypeEnum>().Serialize(info.EntityType, buffer, offset);

        // Position (Vector3)
        offset += TypeSerializers.Vector3.Serialize(info.Position, buffer, offset);

        // Rotation (DirectionEnum)
        offset += TypeSerializers.Enum<DirectionEnum>().Serialize(info.Rotation, buffer, offset);

        // Movement (int)
        offset += TypeSerializers.Int32.Serialize(info.Movement, buffer, offset);

        return offset - startOffset;
    }

    private int DeserializeBase(EntityInfo info, byte[] buffer, int offset)
    {
        int startOffset = offset;

        offset += TypeSerializers.Enum<EntityTypeEnum>().Deserialize(buffer, offset, out EntityTypeEnum entityType);
        info.EntityType = entityType;

        offset += TypeSerializers.Vector3.Deserialize(buffer, offset, out Vector3 position);
        info.Position = position;

        offset += TypeSerializers.Enum<DirectionEnum>().Deserialize(buffer, offset, out DirectionEnum rotation);
        info.Rotation = rotation;

        offset += TypeSerializers.Int32.Deserialize(buffer, offset, out int movement);
        info.Movement = movement;

        return offset - startOffset;
    }

    private int SerializePlayerInfo(PlayerEntityInfo info, byte[] buffer, int offset)
    {
        // For PlayerEntityInfo.Equipment, we need a custom serializer
        // For now, skip equipment or use a placeholder
        // TODO: Implement EquipmentData serializer when needed
        return 0;
    }

    private int DeserializePlayerInfo(PlayerEntityInfo info, byte[] buffer, int offset)
    {
        // TODO: Implement EquipmentData deserializer when needed
        return 0;
    }

    private int SerializeEnemyInfo(EnemyEntityInfo info, byte[] buffer, int offset)
    {
        return TypeSerializers.String.Serialize(info.EntityAssetId, buffer, offset);
    }

    private int DeserializeEnemyInfo(EnemyEntityInfo info, byte[] buffer, int offset)
    {
        int bytesRead = TypeSerializers.String.Deserialize(buffer, offset, out string assetId);
        info.EntityAssetId = assetId;
        return bytesRead;
    }
}

/// <summary>
/// Extension for creating entity-related serializers.
/// </summary>
public static class EntitySerializers
{
    public static readonly ITypeSerializer<EntityInfo> EntityInfo = new EntityInfoSerializer();

    public static ITypeSerializer<System.Collections.Generic.Dictionary<string, EntityInfo>> EntityDictionary()
    {
        return new DictionarySerializer<EntityInfo>(EntityInfo);
    }
}
