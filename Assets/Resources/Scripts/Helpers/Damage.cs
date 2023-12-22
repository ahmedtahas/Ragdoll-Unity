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
            if (collision.gameObject.CompareTag("Head"))
            {
                OnHit?.Invoke(true);
                GameManager.Instance.DamageEnemy(damage * 2, self);
                // collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage * 2);
            }
            else if (collision.gameObject.CompareTag("Damagable"))
            {
                OnHit?.Invoke(false);
                GameManager.Instance.DamageEnemy(damage, self);
                // collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            }
        
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
}