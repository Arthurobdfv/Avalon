using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Combat Base Stats", menuName = "Character/Enemy Combat Base Stats")]
public class EnemyCombatBaseStats : CombatBaseStats
{
    [field: SerializeField] public bool Aggressive { get; private set; }
    [field: SerializeField] public float VisionRange { get; private set; }
}
