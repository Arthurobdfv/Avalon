using System;
using UnityEngine;

public class EnemyCharacter : CombatCharacter
{
    public new EnemyCombatBaseStats BaseStats => (EnemyCombatBaseStats)base.BaseStats;
    private void Start()
    {
        _currentHealth = base.BaseStats.Health;
        _currentTime = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = BaseStats.Aggressive ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, BaseStats.AttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BaseStats.VisionRange);
        if (Target != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, Target.transform.position);
        }
    }

    protected override void OnCombatTick(float delta)
    {
        base.OnCombatTick(delta);
        if (Target != null)
        {

        }
    }

    protected void Update()
    {
        if (Target != null)
        {
            if (!this.IsInRange(Target, BaseStats.AttackRange))
            {
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, BaseStats.MovementSpeed * Time.deltaTime);
            }
        }
    }
}
