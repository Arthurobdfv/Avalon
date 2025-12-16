using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Base Stats", menuName = "Character/Combat Base Stats")]
[Serializable]
public class CombatBaseStats : ScriptableObject
{
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float AttackDamage { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }
}
