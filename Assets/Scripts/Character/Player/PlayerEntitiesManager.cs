using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEntitiesManager : MonoBehaviour
{
    static Dictionary<string, List<PlayerCharacter>> _playersByMap = new Dictionary<string, List<PlayerCharacter>>()
    {
        { Constants.InitialMap, new List<PlayerCharacter>() }
    };

    public static List<PlayerCharacter> PlayersOnMap(string map)
    {
        if (_playersByMap.TryGetValue(map, out var players))
        {
            return players;
        }
        return new List<PlayerCharacter>();
    }

    private void Start()
    {
        _playersByMap.Clear();
        // TODO: Currently only supports InitialMap, need to extend to support multiple maps
        var players = FindObjectsOfType<PlayerCharacter>(true);
        _playersByMap.Add(Constants.InitialMap, players.ToList());
        Debug.Log($"[PlayerEntitiesManager] Found {_playersByMap[Constants.InitialMap].Count} players on map {Constants.InitialMap}");
    }
}
