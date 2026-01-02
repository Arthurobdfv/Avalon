using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private float _lastTick = 0f;
    private float _currentTick = 0f;
    private int _currentTickIndex = 0;
    [SerializeField] private int _tickInterval = 8;
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

    public delegate void OnCombatTick(float previousTickTime, float currentTickTime, float delta);
    public static OnCombatTick CombatTickHandler;

    private void FixedUpdate()
    {
        if (_currentTickIndex >= _tickInterval)
        {
            CombatTickHandler?.Invoke(_lastTick, _currentTick, _currentTick - _lastTick);
            _lastTick = _currentTick;
            _currentTickIndex = 0;
        }
        _currentTick += Time.fixedDeltaTime;
        _currentTickIndex++;
    }

    public void PerformAttack()
    {

    }
}
