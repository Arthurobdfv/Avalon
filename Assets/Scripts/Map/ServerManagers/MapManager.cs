using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static Dictionary<string, HashSet<string>> MapEntities = new Dictionary<string, HashSet<string>>();
    public static HashSet<string> AvailableMaps = new HashSet<string>();

    public static List<string> GetEntities(string mapId = null)
    {
        if (mapId != null)
        {
            if (MapEntities.ContainsKey(mapId))
            {
                return MapEntities[mapId].ToList();
            }
            else
            {
                Debug.LogWarning($"[MapManager] Map ID {mapId} not found in MapEntities.");
                return new List<string>();
            }
        }
        else
        {
            return MapEntities.Values.SelectMany(set => set).Distinct().ToList();
        }
    }

    public static List<string> GetEntityByType(EntityTypeEnum entityType, string mapId = null)
    {
        var allEntities = GetEntities(mapId);
        if(allEntities == null || allEntities.Count == 0)
        {
            return new List<string>();
        }
        return allEntities.Where(x => GlobalEntitiesManager.allEntities[x]?.EntityType == entityType).ToList();

    }
    
    public static string FindMapOfEntity(string entityId)
    {
        foreach (var mapKvp in MapEntities)
        {
            if (mapKvp.Value.Contains(entityId))
            {
                return mapKvp.Key;
            }
        }
        return null;
    }
    [field: SerializeField] private AvailableMaps availableMaps { get; set; }

    private void Awake()
    {
        if (this.availableMaps == null)
        {
            Debug.LogError("[MapManager] AvailableMaps ScriptableObject is not assigned.");
            return;
        }
        var availableMaps = this.availableMaps.MapAvailabilities.Where(map => map.IsMapAvailable).ToList();
        if (availableMaps.Count == 0)
        {
            Debug.LogError("[MapManager] No available maps found in AvailableMaps ScriptableObject.");
        }
        AvailableMaps = availableMaps.Select(av => av.MapId).ToHashSet();
        MapEntities = availableMaps.ToDictionary(avMap => avMap.MapId, avMap => new HashSet<string>());
    }

    private void OnEnable()
    {
        RespawnManager.EntitySpawnedHandler += HandleEntitySpawned;
        RespawnManager.EntityDespawnedHandler += HandleEntityDespawned;
        WarpPointBehavior.OnWarp += WarpPlayer;
    }

    private void OnDisable()
    {
        RespawnManager.EntitySpawnedHandler -= HandleEntitySpawned;
        RespawnManager.EntityDespawnedHandler -= HandleEntityDespawned;
        WarpPointBehavior.OnWarp -= WarpPlayer;
    }

    private void WarpPlayer(WarpPoint point, IEntity entityId)
    {
        if (entityId.EntityType != EntityTypeEnum.Player)
        {
            return;
        }
        var destinationMapId = point.destinationMap.MapID;
        var currentMapId = string.Empty;

        foreach (var mapKvp in MapEntities)
        {
            if (mapKvp.Value.Contains(entityId.Id))
            {
                currentMapId = mapKvp.Key;
                break;
            }
        }
        if (!MapEntities[destinationMapId].Contains(entityId.Id))
        {
            Debug.Log($"Moving {entityId.Id} from {currentMapId} to {destinationMapId}");
            MapEntities[destinationMapId].Add(entityId.Id);
            MapEntities[currentMapId].Remove(entityId.Id);
        }
    }

    private void HandleEntityDespawned(RespawnManager.SpawnEventArgs args)
    {
        if (args.Enemy != null)
        {
            var entityMap = args.MapId;
            MapEntities[entityMap].Remove(args.EntityId);
        }
    }

    private void HandleEntitySpawned(RespawnManager.SpawnEventArgs args)
    {
        if (args.Enemy != null)
        {
            // Add the newly spawned enemy to the appropriate map
            if (!MapEntities.ContainsKey(args.MapId))
            {
                MapEntities[args.MapId] = new HashSet<string>();
            }
            MapEntities[args.MapId].Add(args.EntityId);
            Debug.Log($"[MapManager] Enemy {args.Enemy.name} spawned on map {args.MapId}.");
        }
    }

    private void Update()
    {
        SyncEntities();
    }

    private void SyncEntities()
    {
        var entitiesInMaps = MapEntities.SelectMany(kvp => kvp.Value).ToHashSet();
        foreach (var entity in GlobalEntitiesManager.allEntities)
        {
            if (!entitiesInMaps.Contains(entity.Key))
            {
                if (!MapEntities.ContainsKey(Constants.InitialSpawnMap))
                {
                    MapEntities.Add(Constants.InitialSpawnMap, new HashSet<string>());
                }
                MapEntities[Constants.InitialSpawnMap].Add(entity.Key);
            }
        }
    }
}
