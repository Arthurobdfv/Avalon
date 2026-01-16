using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEntitiesManager : MonoBehaviour
{
    public static List<PlayerCharacter> PlayersOnMap(string map)
    {
        return MapManager.GetEntityByType(EntityTypeEnum.Player, map)
            .Select(x => GlobalEntitiesManager.allEntities[x] as PlayerCharacter)
            .ToList();
    }

    // TODO: PlaceholderMethod to "Locate" and handle player
    public void HandlePlayerInteract(PlayerCharacter character)
    {
        var playerMap = MapManager.FindMapOfEntity(character.Id);
        var enemy = EnemyBehaviourManager.FindClosestEnemy(character.transform.position, playerMap);
        if (enemy != null)
        {
            Debug.Log($"Assigning target {enemy.name} to player {character.name}");
            character.SetTarget(enemy);
        }
    }
}
