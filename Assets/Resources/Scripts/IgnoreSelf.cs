using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSelf : MonoBehaviour
{
    void Start()
    {
        Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            Collider2D[] colliders1 = rigidbodies[i].GetComponents<Collider2D>();
            for (int j = i + 1; j < rigidbodies.Length; j++)
            {
                Collider2D[] colliders2 = rigidbodies[j].GetComponents<Collider2D>();
                foreach (var collider1 in colliders1)
                {
                    foreach (var collider2 in colliders2)
                    {
                        Physics2D.IgnoreCollision(collider1, collider2);
                    }
                }
            }
        }
    }
}
