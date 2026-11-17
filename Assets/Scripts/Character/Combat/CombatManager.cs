using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private float _lastTick = 0f;
    private float _currentTick = 0f;
    private int _currentTickIndex = 0;
    [SerializeField] private int _tickInterval = 8;

    private Stopwatch _stopwatch = new Stopwatch();
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
    public delegate void OnCombatTickEnd(float delta);
    public static OnCombatTickEnd CombatTickEndHandler;
    public delegate void OnBeforeCombatTick(float previousTickTime, float currentTickTime, float delta);
    public static OnBeforeCombatTick BeforeCombatTickHandler;

    private void FixedUpdate()
    {
        if (_currentTickIndex >= _tickInterval)
        {
            _stopwatch.Restart();
            _stopwatch.Start();
            var delta = _currentTick - _lastTick;
            BeforeCombatTickHandler?.Invoke(_lastTick, _currentTick, delta);
            CombatTickHandler?.Invoke(_lastTick, _currentTick, delta);
            CombatTickEndHandler?.Invoke(delta);
            _lastTick = _currentTick;
            _currentTickIndex = 0;
            _stopwatch.Stop();
            //UnityEngine.Debug.Log($"Combat Tick Time: {_stopwatch.ElapsedMilliseconds} ms");
        }
        _currentTick += Time.fixedDeltaTime;
        _currentTickIndex++;
    }

    public void PerformAttack()
    {

    }
}
