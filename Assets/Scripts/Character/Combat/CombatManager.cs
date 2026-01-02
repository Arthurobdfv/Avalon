using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private void OnEnable()
    {
        CombatCharacter.OnPerformCombatHandler += OnPerformCombat;
    }

    private void OnDisable()
    {
        CombatCharacter.OnPerformCombatHandler -= OnPerformCombat;
    }

    private void OnPerformCombat(CombatCharacter.PerformCombatEventArgs combatEventArgs)
    {
        combatEventArgs.TargetCharacter.CurrentHealth -= combatEventArgs.SourceCharacter.BaseStats.AttackDamage;
    }

    public void PerformAttack()
    {

    }
}
