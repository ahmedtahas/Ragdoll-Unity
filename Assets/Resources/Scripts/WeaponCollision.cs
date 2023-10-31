using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    public void UpdateCollisionShape()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
            Destroy(collider);
        }
        gameObject.AddComponent<PolygonCollider2D>();
    }
    public Vector2 GetUpperRightCorner()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
            return collider.bounds.max;
        }
        return Vector2.zero;
    }
}