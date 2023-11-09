using UnityEngine;
using System;

public class Damage : MonoBehaviour
{
    public float damage = 10;
    public event Action OnHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (collision.gameObject.CompareTag("Skill"))
            {
                return;
            }
            OnHit?.Invoke();
            GetComponentInParent<TimeController>().SlowDownTime(0.01f, 0.5f);
            if (collision.gameObject.CompareTag("Head"))
            {
                collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage * 2);
            }
            else if (collision.gameObject.CompareTag("Damagable"))
            {
                collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            }
        
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}