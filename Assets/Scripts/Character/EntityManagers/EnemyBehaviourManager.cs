using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Centralized manager for enemy behaviors, planning on a Multiplayer RPG context
public class EnemyBehaviourManager : MonoBehaviour
{
    public static List<EnemyCharacter> EnemiesOnMap(string map)
    {
        return MapManager
            .GetEntityByType(EntityTypeEnum.Enemy, map)
            .Select(x => GlobalEntitiesManager.allEntities[x] as EnemyCharacter)
            .ToList();
    }

    [SerializeField] int TicksPerRefreshQuery = 20;
    int _currentTick = 0;

    void RefreshEnemyBehaviors()
    {
        foreach (var mapEntry in MapManager.AvailableMaps)
        {
            if(MapManager.GetEntities(mapEntry).Count == 0)
            {
                continue;
            }

            var playersOnMap = MapManager
                .GetEntityByType(EntityTypeEnum.Player, mapEntry)
                .Select(x => GlobalEntitiesManager.allEntities[x] as PlayerCharacter)
                .ToList();

            var enemiesOnMap = MapManager
                .GetEntityByType(EntityTypeEnum.Enemy, mapEntry)
                .Select(x => GlobalEntitiesManager.allEntities[x] as EnemyCharacter)
                .ToList();

            foreach (var enemy in enemiesOnMap)
            {
                HandleEnemyBehavior(enemy, playersOnMap);
            }
        }
    }

    void OnEnable()
    {
        CombatManager.BeforeCombatTickHandler += OnCombatTickStart;
    }

    void OnDisable()
    {
        CombatManager.BeforeCombatTickHandler -= OnCombatTickStart;
    }

    private void OnCombatTickStart(float previousTickTime, float currentTickTime, float delta)
    {
        RefreshEnemyBehaviors();
    }

    void HandleEnemyBehavior(EnemyCharacter enemy, List<PlayerCharacter> playersOnMap)
    {
        if (enemy.Target == null && enemy.IsAvailableForCombat())
        {
            if (enemy.BaseStats.Aggressive)
            {
                //Debug.Log($"[EnemyBehaviourManager] Enemy {enemy.name} is searching for targets.");
                var closestPlayer = playersOnMap.Where(player => EntityExtensions.IsInRange<Character>(enemy, player, enemy.BaseStats.VisionRange))
                    // TODO: Improve target selection logic, possibly sending a list of candidates to a decision-making AI module
                    .ClosestTo(enemy.transform.position);
                if (closestPlayer != null)
                {
                    //Debug.Log($"[EnemyBehaviourManager] Enemy {enemy.name} has found target {closestPlayer.name}.");
                    enemy.SetTarget(closestPlayer);
                }
            }
            else
            {
                // Handle combat behavior
            }
        }

        if (enemy.Target != null)
        {
            // TODO: HandleTarget, checking if still in range, patience level, if is marked for destruction, etc.
            // This needs to be an AI module eventually.
            if (EntityExtensions.IsInRange(enemy, enemy.Target, enemy.BaseStats.AttackRange))
            {
                //enemy.AttackTarget();
                // Attack logic
            }
            else
            {
                // Move towards target logic
            }
        }

    }

    // TODO: Need to improve and convert the multiple parameters into a EnemyQuery structure.
    public static EnemyCharacter FindClosestEnemy(Vector3 position, string mapId, Predicate<EnemyCharacter>? predicate = null)
    {
        IEnumerable<EnemyCharacter> enemies = EnemiesOnMap(mapId);
        if (predicate != null)
        {
            enemies = enemies.Where(x => predicate(x));
        }
        return enemies.ClosestTo(position);
    }


}
