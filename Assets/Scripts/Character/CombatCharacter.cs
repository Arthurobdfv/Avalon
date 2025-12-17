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
            if(value != _currentHealth)
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

    public delegate void OnHealthChange(HealthChangeEventArgs healthChangeEvent);
    public OnHealthChange OnHealthChangeHandler;

    public delegate void PerformCombat(PerformCombatEventArgs combatEventArgs);
    public static PerformCombat OnPerformCombatHandler;
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
