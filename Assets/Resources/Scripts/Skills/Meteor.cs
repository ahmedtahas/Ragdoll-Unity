using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Meteor : MonoBehaviour
{
    float speed = 30.0f;
    Vector3 finalScale = new Vector3(2, 2, 1);
    float finalSpeed = 50f;
    float damage;

    public event Action OnHit;

    void Start()
    {
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().AddToView(transform);
    }

    public void FollowEnemy(GameObject enemy, float duration = 10f, float damage = 12.5f)
    {
        this.damage = damage;
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;

        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && !rb.isKinematic && collision.gameObject.tag != "Skill")
        {
            Health health = collision.gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                if (collision.gameObject.CompareTag("Head"))
                {
                    health.TakeDamage(damage);
                }
                health.TakeDamage(damage);
            }
        }
        OnHit?.Invoke();
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(transform);
        Destroy(gameObject);
    }
}