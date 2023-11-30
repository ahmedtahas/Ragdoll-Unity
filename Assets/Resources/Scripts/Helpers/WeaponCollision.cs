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
            Vector2 maxPoint = collider.points[0];
            foreach (Vector2 point in collider.points)
            {
                if (point.x > maxPoint.x)
                {
                    maxPoint.x = point.x;
                }
                if (point.y > maxPoint.y)
                {
                    maxPoint.y = point.y;
                }
            }
            return maxPoint;
        }
        return Vector2.zero;
    }
}