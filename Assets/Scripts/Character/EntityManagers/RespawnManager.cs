using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    #region Placeholder Contracts
    [Serializable]
    public class EntityLookupData
    {
        [field: SerializeField] EnemyDataEnum enemyDataEnum;
        [field: SerializeField] public string entityID => enemyDataEnum.ToString();
        public GameObject entityPrefab;
    }
    public List<EntityLookupData> entityLookupDatas;
    private Dictionary<string, GameObject> entityLookupDict;
    // Temporary, until we have a proper PlayerData spawner provider implementation

    public List<MapData> mapDatas => availableMaps?.MapAvailabilities.Where(x => x.IsMapAvailable).Select(x => x.MapData).ToList();

    // TODO: This is a placeholder until we have a proper AvailableMaps provider implementation;
    [field:SerializeField] public AvailableMaps availableMaps { get; private set; }

    private Dictionary<MapData, Dictionary<SpawnPointData, (EnemyCharacter enemy, float respawnTime)>> respawnDataDict;
    private Dictionary<EnemyCharacter, SpawnPointData> activeEnemiesDict = new Dictionary<EnemyCharacter, SpawnPointData>();
    private HashSet<EnemyCharacter> enemiesMarkedForDeath = new();
    #endregion

    private void Start()
    {
        entityLookupDict = entityLookupDatas.ToDictionary(x => x.entityID, x => x.entityPrefab);
        respawnDataDict = mapDatas.ToDictionary(
            mapData => mapData,
            mapData => mapData.SpawnPoints.ToDictionary(
                spawnPoint => spawnPoint,
                spawnPoint => (enemy: (EnemyCharacter)null, respawnTime: 0f)
            )
        );
    }

    private void Update()
    {
        foreach (var mapEntry in respawnDataDict)
        {
            foreach (var spawnPointEntry in mapEntry.Value.ToList())
            {
                var spawnPoint = spawnPointEntry.Key;
                var (enemy, respawnTime) = spawnPointEntry.Value;
                if (enemy == null)
                {
                    respawnTime += Time.deltaTime;
                    if (respawnTime >= spawnPoint.RespawnTime)
                    {
                        var entityID = spawnPoint.EntityIDs[RandomNumberGenerator.GetInt32(0, spawnPoint.EntityIDs.Count)].ToString();
                        if (entityLookupDict.TryGetValue(entityID, out var prefab))
                        {
                            var spawnedEnemy = Instantiate(prefab, spawnPoint.SpawnTransform, Quaternion.identity).GetComponent<EnemyCharacter>();
                            spawnedEnemy.EntityAssetId = entityID;
                            mapEntry.Value[spawnPoint] = (spawnedEnemy, 0f);
                            Debug.Log($"Respawned {entityID} at {spawnPoint.SpawnTransform}");
                            var spawnedEntityId = $"en_{spawnedEnemy.GetInstanceID().ToString()}";
                            spawnedEnemy.Id = spawnedEntityId;
                            var eventArgs = new SpawnEventArgs()
                            {
                                Enemy = spawnedEnemy,
                                EntityId = spawnedEnemy.Id,
                                MapId = mapEntry.Key.MapID,
                                SpawnPosition = spawnPoint.SpawnTransform
                            };
                            EntitySpawnedHandler?.Invoke(eventArgs);
                            activeEnemiesDict.Add(spawnedEnemy, spawnPoint);
                        }
                    }
                    else
                    {
                        mapEntry.Value[spawnPoint] = (null, respawnTime);
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        EnemyCharacter.OnMarkedForDeathChangeHandler += AssignToDespawn;
        CombatManager.CombatTickEndHandler += CleanupEntities;
    }

    private void CleanupEntities(float delta)
    {
        foreach (var enemy in enemiesMarkedForDeath)
        {
            if (activeEnemiesDict.TryGetValue(enemy, out var spawnPoint))
            {
                foreach (var mapEntry in respawnDataDict)
                {
                    if (mapEntry.Value.ContainsKey(spawnPoint))
                    {
                        var eventArgs = new SpawnEventArgs()
                        {
                            Enemy = enemy,
                            EntityId = enemy.Id,
                            MapId = mapEntry.Key.MapID,
                            SpawnPosition = spawnPoint.SpawnTransform
                        };
                        EntityDespawnedHandler?.Invoke(eventArgs);
                        mapEntry.Value[spawnPoint] = (null, 0f);
                        break;
                    }
                }
                activeEnemiesDict.Remove(enemy);
            }
            Destroy(enemy.gameObject);
        }
        enemiesMarkedForDeath.Clear();
    }

    private void OnDisable()
    {
        EnemyCharacter.OnMarkedForDeathChangeHandler -= AssignToDespawn;
        CombatManager.CombatTickEndHandler -= CleanupEntities;
    }

    private void AssignToDespawn(CombatCharacter character, bool oldValue, bool newValue)
    {
        if (newValue)
        {
            var enemy = character as EnemyCharacter;
            if (!enemiesMarkedForDeath.Contains(enemy))
            {
                enemiesMarkedForDeath.Add(enemy);
            }
        }
    }

    public delegate void OnEntitySpawned(SpawnEventArgs args);

    public static event OnEntitySpawned EntitySpawnedHandler;

    public delegate void OnEntityDespawned(SpawnEventArgs args);
    public static event OnEntityDespawned EntityDespawnedHandler;

    public class SpawnEventArgs : EventArgs
    {
        public EnemyCharacter Enemy { get; set; }
        public string EntityId { get; set; }
        public string MapId { get; set; }
        public Vector2 SpawnPosition { get; set; }
    }
}