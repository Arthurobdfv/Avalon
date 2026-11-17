using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityExtensions
{
    public static T ClosestTo<T>(this IEnumerable<T> entities, Vector3 position) where T : Character
    {
        if(entities != null && entities.Any() == false)
        {
            return null;
        }
        return entities.Aggregate((p1, p2) => (position - p1.transform.position).sqrMagnitude < (position - p2.transform.position).sqrMagnitude ? p1 : p2);
    }

    // TODO: Move these methods to a separate file
    public static bool IsInRange<T>(this T source, T target, float range) where T : Character
    {
        try
        {
            float distance = Vector3.SqrMagnitude(source.transform.position - target.transform.position);
            return distance <= (range * range);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in IsInRange: {e.Message}");
            return false;
        }
    }

    public static bool IsAvailableForCombat(this CombatCharacter character, Func<bool> extraValidations = null)
    {
        if (character == null || character.MarkedForDeath)
        {
            return false;
        }
        if(extraValidations != null && !extraValidations())
        {
            return false;
        }
        return true;
    }
}
