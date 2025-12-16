using UnityEngine;

public class CombatCharacter : Character
{
    [SerializeField] protected CombatBaseStats BaseStats;
    public Character Target { get; protected set; }

    protected float _currentHealth;
    public float CurrentHealth  => _currentHealth;
    protected float _currentTime;

    public void SetTarget(Character target)
    {
        Target = target;
    }
}
