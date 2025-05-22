using UnityEngine;
using System;

public class Damage : MonoBehaviour
{
    public float damage = 10;
    public event Action<bool> OnHit;
    GameObject self;

    void OnEnable()
    {
        if (self == null) self = transform.GetComponentInParent<CharacterManager>().gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (collision.gameObject.CompareTag("Skill"))
            {
                return;
            }

            GameObject targetObject = collision.gameObject;

            // Find the Health component of the target
            Health targetHealth = collision.transform.GetComponentInParent<Health>();

            if (collision.gameObject.CompareTag("Head"))
            {
                OnHit?.Invoke(true);

                // Store the hit part in the health component
                if (targetHealth != null)
                {
                    targetHealth.lastDamagedPart = collision.gameObject;
                }

                GameManager.Instance.DamageEnemy(damage * 2, self);

                // Flash the specific body part (head)
                ApplyDamageFlash(targetObject, 1.0f);
            }
            else if (collision.gameObject.CompareTag("Damagable"))
            {
                OnHit?.Invoke(false);

                // Store the hit part in the health component
                if (targetHealth != null)
                {
                    targetHealth.lastDamagedPart = collision.gameObject;
                }

                GameManager.Instance.DamageEnemy(damage, self);

                // Flash the specific body part
                ApplyDamageFlash(targetObject, 0.7f);
            }
        }
    }

    // Apply a temporary red flash effect to the specific body part
    private void ApplyDamageFlash(GameObject bodyPart, float intensity)
    {
        // Find the body object
        Transform bodyTransform = bodyPart.transform;
        while (bodyTransform != null && bodyTransform.name != "Body")
        {
            bodyTransform = bodyTransform.parent;
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

    public void SetSelf(GameObject self)
    {
        this.self = self;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetDamage()
    {
        return damage;
    }
}