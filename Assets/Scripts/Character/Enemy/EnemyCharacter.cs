using System;
using UnityEngine;

public class EnemyCharacter : Character
{
    // TODO: Move this to a new file, currently hardcoded few values for testing
    [Serializable]
    public class EnemyBehavior
    {
        [field: SerializeField] public bool Aggressive { get; set; }
        [field: SerializeField] public bool Ranged { get; set; }
        float MeleeRange = 5f;
        float RangedRange = 15f;
        [SerializeField] public float Range => Ranged ? RangedRange : MeleeRange;
    }
    public Character Target { get; protected set; } 
    public EnemyBehavior Behavior => _behavior;
    [SerializeField] EnemyBehavior _behavior = null;

    public void SetTarget(Character target)
    {
        Target = target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Behavior.Aggressive ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Behavior.Range);
        if(Target != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, Target.transform.position);
        }
    }
}
