using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private float speed = 300f;
    public int damage = 10;
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
}