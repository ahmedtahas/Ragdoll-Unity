using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public int damageAmount = 10;

    void Start()
    {
        GameObject.Find("MultiTargetCamera").GetComponent<MultiTargetCamera>().AddToView(transform);
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    { 
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                if (collision.gameObject.CompareTag("Skill"))
                {
                    Debug.Log("Bullet hit a skill");
                }
                else if (collision.gameObject.CompareTag("Head"))
                {
                    collision.gameObject.GetComponentInParent<Health>().TakeDamage(damageAmount * 2);
                }
                else
                {
                    collision.gameObject.GetComponentInParent<Health>().TakeDamage(damageAmount);
                }
            }
        }
        GameObject.Find("MultiTargetCamera").GetComponent<MultiTargetCamera>().RemoveFromView(transform);
        Destroy(gameObject);
    }
}