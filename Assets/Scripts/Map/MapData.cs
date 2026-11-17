using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RespawnManager;

[CreateAssetMenu(fileName = "new MapData", menuName = "MapData/MapData")]
public class MapData : ScriptableObject
{
    [field: SerializeField] public string MapID { get; private set; }
    [field: SerializeField] public List<SpawnPointData> SpawnPoints { get; private set; }
    [field: SerializeField] public List<WarpPoint> WarpPoints { get; private set; }
}
