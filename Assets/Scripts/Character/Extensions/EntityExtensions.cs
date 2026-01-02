using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityExtensions
{
    public static T ClosestTo<T>(this IEnumerable<T> entities, Vector3 position) where T : Character
    {
        return entities.Aggregate((p1, p2) => (position - p1.transform.position).sqrMagnitude < (position - p2.transform.position).sqrMagnitude ? p1 : p2);
    }
}
