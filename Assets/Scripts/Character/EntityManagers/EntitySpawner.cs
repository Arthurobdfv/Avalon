using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static EntitySpawner;

public class EntitySpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnedEntity
    {
        public string AssetId;
        public EntityUI UIComponent;
        public EntityTypeEnum EntityType;
    }


    [field: SerializeField] EntityUI EntityUIPrefab;
    [field: SerializeField] Character CharacterPrefab;

    [field: SerializeField] public Dictionary<string, SpawnedEntity> SpawnedEntities { get; private set; } = new Dictionary<string, SpawnedEntity>();
    [field: SerializeField] public Dictionary<string, Character> Characters { get; private set; } = new Dictionary<string, Character>();


    private ClientPacketHandler _clientPacketHandler;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if(_clientPacketHandler == null)
        {
            _clientPacketHandler = FindAnyObjectByType<ClientPacketHandler>();
        }
        _clientPacketHandler.RegisterClientHandler<EntitySpawnPacket>(SpawnEntities);
    }

    private void SpawnEntities(EntitySpawnPacket packet)
    {
        List<string> KeysToRemove = new List<string>();
        foreach (var spawnedEntity in SpawnedEntities)
        {
            if (!packet.Entities.ContainsKey(spawnedEntity.Key))
            {
                if (spawnedEntity.Value.EntityType == EntityTypeEnum.Player)
                {
                    UpdateCharacter(spawnedEntity.Key, packet.Entities[spawnedEntity.Key]);
                    Destroy(spawnedEntity.Value.UIComponent.gameObject);

                }
                else
                {
                    Debug.Log($"Despawning Entity with AssetId {spawnedEntity.Key}");
                    Destroy(spawnedEntity.Value.UIComponent.gameObject);
                    KeysToRemove.Add(spawnedEntity.Key);
                }
            }
        }
        foreach (var key in KeysToRemove)
        {
            SpawnedEntities.Remove(key);
        }
        foreach (var entity in packet.Entities)
        {
            // TODO handle player
            if (SpawnedEntities.ContainsKey(entity.Key) || Characters.ContainsKey(entity.Key))
            {
                if (entity.Value.EntityType == EntityTypeEnum.Player)
                {
                    UpdateCharacter(entity.Key, entity.Value);
                }
                else
                {
                    //Debug.LogWarning($"Entity with AssetId {entity.Key} already spawned. Handling Update");
                    UpdateEntity(entity.Key, entity.Value);
                    var entityUI = SpawnedEntities[entity.Key];
                }
            }
            else
            {
                if (entity.Value.EntityType == EntityTypeEnum.Player)
                {
                    SpawnCharacter(entity.Key, entity.Value);
                }
                else
                {
                    //Debug.Log($"Spawning Entity with AssetId {entity.Value.EntityAssetId}");
                    SpawndEntity(entity.Key, entity.Value);
                }
            }
        }
    }

    private void SpawndEntity(string key, EntityInfo value)
    {
        var entity = Instantiate(EntityUIPrefab, value.Position, Quaternion.identity);
        var ui = entity.GetComponent<EntityUI>();
        ui.SpawnUI(value.EntityAssetId);
        SpawnedEntity spawnedEntity = new SpawnedEntity()
        {
            AssetId = value.EntityAssetId,
            UIComponent = entity,
            EntityType = value.EntityType
        };
        SpawnedEntities.Add(key, spawnedEntity);
    }

    private void SpawnCharacter(string key, EntityInfo value)
    {
        var character = Instantiate(CharacterPrefab, value.Position, Quaternion.identity);
        Characters.Add(key, character);
    }

    private void UpdateCharacter(string key, EntityInfo value)
    {
        var character = Characters[key];
        character.transform.position = value.Position;
        character.SetDirection(value.Rotation);
        character.SetMovement(value.Movement);
    }

    private void UpdateEntity(string key, EntityInfo value)
    {
        SpawnedEntities[key].UIComponent.gameObject.transform.position = value.Position;
    }

    private void OnDisable()
    {
        _clientPacketHandler.UnregisterClientHandler<EntitySpawnPacket>();
    }
}
