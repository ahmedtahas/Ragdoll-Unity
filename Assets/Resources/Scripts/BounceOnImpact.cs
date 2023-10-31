using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnImpact : MonoBehaviour
{
    public float bounceForce = 5f; // The force applied when bouncing

    private Rigidbody2D[] siblingRigidbodies;

    void Start()
    {
        // Get all sibling rigidbodies
        siblingRigidbodies = transform.parent.GetComponentsInChildren<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D collidedRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        // Check if the collided object is a dynamic Rigidbody2D
        if (collidedRigidbody != null && collidedRigidbody.bodyType == RigidbodyType2D.Dynamic)
        {
            // Calculate bounce direction
            Vector2 contactNormal = collision.contacts[0].normal;
            float power = 10f; 

            // Apply impulse to this object
            GetComponent<Rigidbody2D>().AddForce(contactNormal * bounceForce, ForceMode2D.Impulse);

            // Apply impulse to all siblings
            foreach (Rigidbody2D siblingRb in siblingRigidbodies)
            {
                siblingRb.AddForce(contactNormal * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}