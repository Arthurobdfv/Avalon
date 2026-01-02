using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static EntityExtensions;

// Centralized manager for enemy behaviors, planning on a Multiplayer RPG context
public class EnemyBehaviourManager : MonoBehaviour
{
    static Dictionary<string, List<EnemyCharacter>> _enemiesByMap = new Dictionary<string, List<EnemyCharacter>>()
    {
        { Constants.InitialMap, new List<EnemyCharacter>() }
    };

    public static List<EnemyCharacter> EnemiesOnMap(string map)
    {
        if (_enemiesByMap.TryGetValue(map, out var enemies))
        {
            return enemies;
        }
        return new List<EnemyCharacter>();
    }

    [SerializeField] int TicksPerRefreshQuery = 20;
    int _currentTick = 0;

    // Start is called before the first frame update
    void Start()
    {
        _enemiesByMap.Clear();
        // TODO: Currently only supports InitialMap, need to extend to support multiple maps
        var enemies = FindObjectsOfType<EnemyCharacter>(true);
        _enemiesByMap.Add(Constants.InitialMap, enemies.ToList());
        Debug.Log($"[EnemyBehaviourManager] initialized with {_enemiesByMap[Constants.InitialMap].Count} enemies on map {Constants.InitialMap}");
    }

    // Update is called once per frame
    void Update()
    {
        _currentTick++;
        if (_currentTick >= TicksPerRefreshQuery)
        {
            RefreshEnemyBehaviors();
            _currentTick = 0;
        }
    }

    void RefreshEnemyBehaviors()
    {
        foreach (var mapEntry in _enemiesByMap)
        {
            var playersOnMap = PlayerEntitiesManager.PlayersOnMap(mapEntry.Key);
            string mapName = mapEntry.Key;
            List<EnemyCharacter> enemies = mapEntry.Value;
            foreach (var enemy in enemies)
            {
                HandleEnemyBehavior(enemy, playersOnMap);
            }
        }
    }

    void HandleEnemyBehavior(EnemyCharacter enemy, List<PlayerCharacter> playersOnMap)
    {
        if (enemy.Target == null)
        {
            if (enemy.BaseStats.Aggressive)
            {
                Debug.Log($"[EnemyBehaviourManager] Enemy {enemy.name} is searching for targets.");
                var closestPlayer = playersOnMap.Where(player => EntityExtensions.IsInRange<Character>(enemy, player, enemy.BaseStats.VisionRange))
                    // TODO: Improve target selection logic, possibly sending a list of candidates to a decision-making AI module
                    .ClosestTo(enemy.transform.position);
                if (closestPlayer != null)
                {
                    Debug.Log($"[EnemyBehaviourManager] Enemy {enemy.name} has found target {closestPlayer.name}.");
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

        if (enemy.CurrentHealth <= 0)
        {

        }
    }

    // TODO: Need to improve and convert the multiple parameters into a EnemyQuery structure.
    public static EnemyCharacter FindClosestEnemy(Vector3 position, string mapId, Predicate<EnemyCharacter>? predicate = null)
    {
        IEnumerable<EnemyCharacter> enemies = EnemiesOnMap(mapId);
        if(predicate != null)
        {
            enemies = enemies.Where(x => predicate(x));
        }
        return enemies.ClosestTo(position);
    }
}
