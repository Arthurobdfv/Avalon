using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalEntitiesManager : MonoBehaviour
{
    PlayerEntitiesManager playerEntitiesManager;
    EnemyBehaviourManager enemyBehaviourManager;

    public static Dictionary<string, IEntity> allEntities = new Dictionary<string, IEntity>();

    ServerCommunicationLayerManager serverCommunicationLayerManager = null;

    public PlayerCharacter PlayerPrefab;

    ServerCommunicationLayerManager ServerCommunicationLayerManager
    {
        get
        {
            if (serverCommunicationLayerManager == null)
            {
                serverCommunicationLayerManager = FindObjectOfType<ServerCommunicationLayerManager>();
            }
            return serverCommunicationLayerManager;
        }
    }

    private void OnEnable()
    {
        CombatCharacter.OnHealthReachZeroHandler += OnEntityDeath;
        RespawnManager.EntitySpawnedHandler += HandleEntitySpawned;
        RespawnManager.EntityDespawnedHandler += HandleEntityDespawned;
        ServerCommunicationLayerManager.Handler.RegisterServerHandler<PlayerEquipmentPacket>(OnPlayerEquipChange);
    }

    private void OnPlayerEquipChange(PlayerEquipmentPacket packet)
    {
        var playerCharacter = allEntities[packet.ClientId] as PlayerCharacter;
        if (playerCharacter != null)
        {
            playerCharacter.Equipment.BaseBody = packet.PlayerBaseAsset;
        }
        else
        {
            throw new Exception($"Player with ID {packet.ClientId} not found in GlobalEntitiesManager or is not assignable to PlayerCharacter.");
        }
    }

    private void HandleEntityDespawned(RespawnManager.SpawnEventArgs args)
    {
        allEntities.Remove(args.EntityId);
    }

    private void HandleEntitySpawned(RespawnManager.SpawnEventArgs args)
    {
        if (allEntities.ContainsKey(args.EntityId))
        {
            Debug.LogError($"Entity with ID {args.EntityId} already exists in GlobalEntitiesManager.");
        }
        allEntities.Add(args.EntityId, args.Enemy);
    }

    private void OnDisable()
    {
        CombatCharacter.OnHealthReachZeroHandler -= OnEntityDeath;
        RespawnManager.EntitySpawnedHandler -= HandleEntitySpawned;
        RespawnManager.EntityDespawnedHandler -= HandleEntityDespawned;
        serverCommunicationLayerManager.Handler.UnregisterServerHandler<PlayerEquipmentPacket>();

    }

    private void OnEntityDeath(CombatCharacter character)
    {
        if (!character.MarkedForDeath)
        {
            if (character is EnemyCharacter)
            {
                var enemy = character as EnemyCharacter;
                Debug.Log($"Enemy {enemy.name} has died.");
                enemy.MarkedForDeath = true;
            }
            else if (character is PlayerCharacter)
            {
                var player = character as PlayerCharacter;
                Debug.Log($"Player {player.name} has died.");
                player.MarkedForDeath = true;
            }
        }
    }

    public void OnPlayerConnect(ConnectPlayerPacket packet)
    {
        var player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        player.Id = packet.PlayerId;
        allEntities.Add(player.Id, player);
    }


    // TODO: Change this to FixedUpdate if necessary or to follow the server tick rate
    private void LateUpdate()
    {
        foreach (var map in MapManager.AvailableMaps)
        {
            var currentMap = map; // TODO: Get current map from player or game state
            var playersOnMap = PlayerEntitiesManager.PlayersOnMap(currentMap).ToDictionary(x => x.Id, x => (EntityInfo)new PlayerEntityInfo() { EntityType = EntityTypeEnum.Player, Position = x.transform.position, Rotation = x.CurrentDiretion, Movement = x.CurrentMovement, Equipment = x.Equipment });
            var enemiesOnMap = EnemyBehaviourManager.EnemiesOnMap(currentMap).ToDictionary(x => x.Id, x => (EntityInfo)new EnemyEntityInfo() { EntityType = EntityTypeEnum.Enemy, Position = x.transform.position, Rotation = x.CurrentDiretion, Movement = x.CurrentMovement, EntityAssetId = x.EntityAssetId });

            Dictionary<string, EntityInfo> allEntities = playersOnMap.Concat(enemiesOnMap).ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value);
            var packet = new EntitySpawnPacket()
            {
                Entities = allEntities,
                MapId = currentMap
            };

            ServerCommunicationLayerManager.SendMap(packet, map);
        }
    }

    public delegate void OnPlayerEntityConnected(string playerId);
}
