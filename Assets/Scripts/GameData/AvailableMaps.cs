using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailableMaps", menuName = "Game Data/Available Maps")]
public class AvailableMaps : ScriptableObject
{
    [field: SerializeField] public List<MapAvailability> MapAvailabilities { get; private set; } = new List<MapAvailability>();
}   
