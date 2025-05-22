using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Meteor : MonoBehaviour
{
    float speed = 30.0f;
    Vector3 finalScale = new Vector3(2, 2, 1);
    float finalSpeed = 50f;
    public float damage;
    Health health;
    public event Action<bool, bool> OnHit;

    void Start()
    {
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().AddToView(transform);
    }

    public void FollowEnemy(GameObject owner, GameObject enemy, float duration, float damage)
    {
        this.damage = damage;
        health = owner.GetComponent<Health>();
        StartCoroutine(FollowAndScale(enemy, duration));
    }

    private IEnumerator FollowAndScale(GameObject enemy, float duration = 10f)
    {
        Vector3 initialScale = transform.localScale;
        float initialSpeed = speed;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            speed = Mathf.Lerp(initialSpeed, finalSpeed, elapsedTime / duration);

            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);

            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        speed = finalSpeed;
        Vector3 direction = (enemy.transform.position - transform.position).normalized;

        // Continue moving in the same direction until the meteor hits something
        while (true)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic && collision.gameObject.tag != "Skill" && collision.gameObject.tag != "Shield")
        {
            Health targetHealth = collision.gameObject.GetComponentInParent<Health>();

            // Apply a very intense damage flash for meteor (most powerful weapon)
            ApplyDamageFlash(collision.gameObject, collision.gameObject.CompareTag("Head") ? 1.5f : 1.2f);

            // Store hit part in Health component for staged death
            if (targetHealth != null)
            {
                targetHealth.lastDamagedPart = collision.gameObject;
            }

            if (health == targetHealth)
            {
                if (collision.gameObject.CompareTag("Head"))
                {
                    health.Heal(-damage);
                }
                health.Heal(-damage);
                OnHit?.Invoke(false, false);
            }
            else
            {
                if (collision.gameObject.CompareTag("Head"))
                {
                    OnHit?.Invoke(true, true);
                }
                else
                {
                    OnHit?.Invoke(true, false);
                }
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

            // Flash the specific body part with orange-red for meteor (fire effect)
            flashComponent.FlashBodyPart(bodyPart, intensity, new Color(1f, 0.4f, 0.1f, 1f));
        }
    }
}