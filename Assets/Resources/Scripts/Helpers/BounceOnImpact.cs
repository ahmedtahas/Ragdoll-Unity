using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BounceOnImpact : MonoBehaviour
{
    private float bounceForce = 15f; // The force applied when bouncing
    public Rigidbody2D[] siblingRigidbodies;
    public event Action OnBounce;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D collidedRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        // Check if the collided object is a dynamic Rigidbody2D
        if (collidedRigidbody != null && collidedRigidbody.bodyType == RigidbodyType2D.Dynamic)
        {
            Bounce(collision.contacts[0].normal);
        }
    }

    public void Bounce(Vector2 direction)
    {
        OnBounce?.Invoke();
        GetComponent<Rigidbody2D>().AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);

        // Apply impulse to all siblings
        foreach (Rigidbody2D siblingRb in siblingRigidbodies)
        {
            siblingRb.AddForce(direction * bounceForce, ForceMode2D.Impulse);
        }
    }

    public void SetKnockback(float knockback)
    {
        bounceForce = knockback;
    }
}