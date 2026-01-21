using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnPointData", menuName = "MapData/SpawnPointData", order = 1)]
public class SpawnPointData : ScriptableObject
{
    [field: SerializeField] public float RespawnTime { get; private set; } = 5f;
    [field: SerializeField] public Vector2 SpawnTransform { get; private set; }
    [field: SerializeField] public List<EnemyDataEnum> EntityIDs { get; private set; }
}
