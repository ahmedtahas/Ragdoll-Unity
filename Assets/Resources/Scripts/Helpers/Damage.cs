using UnityEngine;
using System;

public class Damage : MonoBehaviour
{
    public float damage = 10;
    public event Action OnHit;
    GameObject self;

    void OnEnable()
    {
        self = transform.GetComponentInParent<CharacterManager>().gameObject;
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
            OnHit?.Invoke();
            if (collision.gameObject.CompareTag("Head"))
            {
                GameManager.Instance.DamageEnemy(damage * 2, self);
                // collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage * 2);
            }
            else if (collision.gameObject.CompareTag("Damagable"))
            {
                GameManager.Instance.DamageEnemy(damage, self);
                // collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            }
        
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}