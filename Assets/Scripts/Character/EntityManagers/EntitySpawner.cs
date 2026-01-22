using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks.Sources;
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

    public class SpawnedEntityInfo
    {
        public EntityTypeEnum EntityType;
        public EntityInfo Info;
        public EntityUI UIComponent;
        public Character SpawnedEntityCharacter;
    }


    [field: SerializeField] EnemyEntityUI EntityUIPrefab;
    [field: SerializeField] PlayerUI CharacterPrefab;

    [field: SerializeField] public Dictionary<string, SpawnedEntity> SpawnedEntities { get; private set; } = new Dictionary<string, SpawnedEntity>();
    [field: SerializeField] public Dictionary<string, SpawnedEntityInfo> Characters { get; private set; } = new Dictionary<string, SpawnedEntityInfo>();


    private ClientCommunicationLayerManager _clientCommunicationLayerManager;

    ClientCommunicationLayerManager ClientCommunicationLayerManager
    {
        get
        {
            if (_clientCommunicationLayerManager == null)
            {
                _clientCommunicationLayerManager = FindAnyObjectByType<ClientCommunicationLayerManager>();
            }
            return _clientCommunicationLayerManager;
        }
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        ClientCommunicationLayerManager.Handler.RegisterClientHandler<EntitySpawnPacket>(SpawnEntities);
        ClientCommunicationLayerManager.Handler.RegisterClientHandler<PlayerEquipmentPacket>(UpdatePlayerEquipment);
    }

    private void UpdatePlayerEquipment(PlayerEquipmentPacket packet)
    {
        var playerUIComponent = Characters[packet.ClientId].UIComponent as PlayerUI;
        if (playerUIComponent == null)
        {
            throw new Exception("Player UI Component is not of type PlayerUI");
        }
        playerUIComponent.UpdatePlayerBaseAsset(packet.PlayerBaseAsset);
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
        var ui = entity.GetComponent<EnemyEntityUI>();
        var enemyInfo = value as EnemyEntityInfo;
        if (enemyInfo == null)
        {
            throw new Exception("EntityInfo is not of type EnemyEntityInfo");
        }
        ui.SetEntityAssetId(enemyInfo.EntityAssetId);
        SpawnedEntity spawnedEntity = new SpawnedEntity()
        {
            AssetId = enemyInfo.EntityAssetId,
            UIComponent = entity,
            EntityType = value.EntityType
        };
        SpawnedEntities.Add(key, spawnedEntity);
    }

    private void SpawnCharacter(string key, EntityInfo value)
    {
        var character = Instantiate(CharacterPrefab, value.Position, Quaternion.identity) ;
        var spawnedCharacterInfo = new SpawnedEntityInfo()
        {
            EntityType = value.EntityType,
            Info = value,
            UIComponent = character,
            SpawnedEntityCharacter = character.GetComponent<Character>() ?? throw new Exception("Character prefab does not have a Character component")
        };
        Characters.Add(key, spawnedCharacterInfo);
    }

    private void UpdateCharacter(string key, EntityInfo value)
    {
        var playerEntityInfo = value as PlayerEntityInfo;
        var character = Characters[key].SpawnedEntityCharacter;
        character.transform.position = value.Position;
        character.SetDirection(value.Rotation);
        character.SetMovement(value.Movement);
        (Characters[key].UIComponent as PlayerUI).UpdatePlayerBaseAsset(playerEntityInfo.Equipment.BaseBody);
    }

    private void UpdateEntity(string key, EntityInfo value)
    {
        SpawnedEntities[key].UIComponent.gameObject.transform.position = value.Position;
    }

    private void OnDisable()
    {
        ClientCommunicationLayerManager.Handler.UnregisterClientHandler<EntitySpawnPacket>();
    }
}
