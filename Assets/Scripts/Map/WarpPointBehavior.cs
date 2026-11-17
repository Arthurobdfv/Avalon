using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPointBehavior : MonoBehaviour
{
    [SerializeField] private WarpPoint warpPoint;

    public delegate void WarpAction(WarpPoint point, IEntity entityId);
    public static event WarpAction OnWarp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IEntity entity))
        {
            Debug.Log($"Entity {entity.Id} entered warp point {warpPoint.name}");
            OnWarp?.Invoke(warpPoint, entity);
        }
    }
}
