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
        if (collidedRigidbody != null && collidedRigidbody.bodyType == RigidbodyType2D.Dynamic)
        {
            Bounce(collision.contacts[0].normal);
        }
    }

    public void Bounce(Vector2 direction)
    {
        GetComponentInParent<TimeController>().SlowDownTime(0.02f, 0.5f);
        Vector2 newDirection = new Vector3(direction.x, direction.y, 0).normalized;
        OnBounce?.Invoke();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(newDirection.normalized * bounceForce, ForceMode2D.Impulse);

        // foreach (Rigidbody2D siblingRb in siblingRigidbodies)
        // {
        //     siblingRb.velocity = Vector2.zero;
        //     siblingRb.angularVelocity = 0f;
        // }
        foreach (Rigidbody2D siblingRb in siblingRigidbodies)
        {
            siblingRb.AddForce(newDirection.normalized * bounceForce, ForceMode2D.Impulse);
        }
    }

    public void SetKnockback(float knockback)
    {
        bounceForce = knockback;
    }
}