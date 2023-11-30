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
        if (name == "Head")
            StartCoroutine(CameraShake(0.03f, 0.1f));
        OnBounce?.Invoke();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);
        foreach (Rigidbody2D siblingRb in siblingRigidbodies)
        {
            siblingRb.AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);
        }
    }

    public void Pushed(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        // foreach (Rigidbody2D siblingRb in siblingRigidbodies)
        // {
        //     siblingRb.velocity = Vector2.zero;
        //     siblingRb.angularVelocity = 0f;
        // }
        foreach (Rigidbody2D siblingRb in siblingRigidbodies)
        {
            siblingRb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            GameObject.Find(Constants.MTC).transform.localPosition += new Vector3(x, y, -1);

            elapsed += Time.deltaTime;

            yield return null;
        }

    }

    public void SetKnockback(float knockback)
    {
        bounceForce = knockback;
    }
}