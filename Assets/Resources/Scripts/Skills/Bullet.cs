using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private float speed = 300f;
    public int damage = 200;
    public event Action<Vector3, bool> OnHit;

    void Start()
    {
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().AddToView(transform);
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic && collision.gameObject.tag != "Skill" && collision.gameObject.tag != "Shield")
        {
            // Apply damage flash
            ApplyDamageFlash(collision.gameObject, collision.gameObject.CompareTag("Head") ? 1.0f : 0.7f);

            // Store hit part in Health component for staged death
            Health targetHealth = collision.transform.GetComponentInParent<Health>();
            if (targetHealth != null)
            {
                targetHealth.lastDamagedPart = collision.gameObject;
            }

            if (collision.gameObject.CompareTag("Head"))
            {
                OnHit?.Invoke(transform.position, true);
            }
            else
            {
                OnHit?.Invoke(transform.position, false);
            }
        }
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(transform);
        Destroy(gameObject);
    }

    // Apply a damage flash to the hit body part
    private void ApplyDamageFlash(GameObject bodyPart, float intensity)
    {
        // Find the body object
        Transform bodyTransform = bodyPart.transform;
        while (bodyTransform != null && bodyTransform.name != "Body")
        {
            bodyTransform = bodyTransform.parent;
            if (bodyTransform == null) break;
        }

        // If we found the body
        if (bodyTransform != null)
        {
            // Get or add the DamageFlash component
            DamageFlash flashComponent = bodyTransform.GetComponent<DamageFlash>();
            if (flashComponent == null)
            {
                flashComponent = bodyTransform.gameObject.AddComponent<DamageFlash>();
            }

            // Flash the specific body part
            flashComponent.FlashBodyPart(bodyPart, intensity);
        }
    }
}