using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 300f;
    public int damage = 10;

    void Start()
    {
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().AddToView(transform);
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    { 
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (!rb.isKinematic && collision.gameObject.tag != "Skill")
            {
                Health health = collision.gameObject.GetComponentInParent<Health>();
                if (health != null)
                {
                    if (collision.gameObject.CompareTag("Head"))
                    {
                        health.TakeDamage(damage);
                    }
                    Debug.Log("Meteor hit " + collision.gameObject.name);
                    health.TakeDamage(damage);
                }
            }
        }
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(transform);
        Destroy(gameObject);
    }
}