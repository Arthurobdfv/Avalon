using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"EntityCollisionDetector: OnTriggerEnter2D with {collision.gameObject.name}");
    }
}
