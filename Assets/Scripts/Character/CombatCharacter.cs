using System;
using UnityEngine;

public class CombatCharacter : Character
{
    [SerializeField] private CombatBaseStats _baseStats;
    public CombatCharacter Target { get; protected set; }
    public CombatBaseStats BaseStats => _baseStats;

    protected float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value != _currentHealth)
            {
                var healthChangeEventArgs = new HealthChangeEventArgs
                {
                    MaxHealth = _baseStats.Health,
                    NewHealth = value,
                    OldHealth = _currentHealth
                };

                _currentHealth = value;
                OnHealthChangeHandler?.Invoke(healthChangeEventArgs);
            }
        }
    }
    protected float _currentTime;

    public void SetTarget(CombatCharacter target)
    {
        Target = target;
    }

    protected virtual void Awake()
    {
        _currentHealth = BaseStats.Health;
    }

    private void OnEnable()
    {
        CombatManager.CombatTickHandler += HandleCombatTick;
    }

    private void HandleCombatTick(float previousTickTime, float currentTickTime, float delta)
    {
        if (Target != null)
        {
            _currentTime += delta;
            Debug.Log($"[CombatCharacter] {_currentTime} / {BaseStats.AttackSpeed}");
        }
        OnCombatTick(delta);
    }

    private void OnDisable()
    {
        CombatManager.CombatTickHandler -= HandleCombatTick;
    }

    protected virtual void OnCombatCharacterEnable() { }
    protected virtual void OnCombatTick(float delta)
    {
        if (Target != null)
        {
            if (_currentTime > BaseStats.AttackSpeed && this.IsInRange(Target, BaseStats.AttackRange))
            {
                OnPerformCombatHandler?.Invoke(new PerformCombatEventArgs { SourceCharacter = this, TargetCharacter = Target });
                _currentTime = 0f;
            }
        }
    }
    protected virtual void OnCombatCharacterDisable() { }

    public delegate void OnHealthChange(HealthChangeEventArgs healthChangeEvent);
    public OnHealthChange OnHealthChangeHandler;

    public delegate void PerformCombat(PerformCombatEventArgs combatEventArgs);
    public static PerformCombat OnPerformCombatHandler;

    public void OnHealthReachZero()
    {
        SetTarget(null);
        // TODO: Play death animation
    }

    // TODO: Move these to a different file
    public class HealthChangeEventArgs
    {
        public float OldHealth;
        public float NewHealth;
        public float MaxHealth;
    }
    public class PerformCombatEventArgs
    {
        public CombatCharacter SourceCharacter;
        public CombatCharacter TargetCharacter;
    }
}
