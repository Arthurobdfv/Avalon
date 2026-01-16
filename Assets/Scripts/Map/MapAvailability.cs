using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new MapAvailability", menuName = "MapData/MapAvailability")]
[Serializable]
public class MapAvailability
{
    [field: SerializeField] public bool IsMapAvailable { get; private set; }
    [field: SerializeField] public MapData MapData { get; private set; }
    public string MapId => MapData?.MapID;
}
