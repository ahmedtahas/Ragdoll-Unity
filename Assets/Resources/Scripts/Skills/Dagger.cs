using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dagger : MonoBehaviour
{
    private MultiTargetCamera cam;
    public float maxDistance = 100f;
    private Vector3 startPosition;
    private Stele player;
    public int damage = 10;
    public float spinSpeed = 5f;

    public event Action<Vector3> OnHit;

    void Start()
    {
        cam = GameObject.Find("MultiTargetCamera").GetComponent<MultiTargetCamera>();
        startPosition = transform.position;
        player = FindObjectOfType<Stele>();
        cam.AddToView(transform);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
        float distanceTraveled = ((Vector2)startPosition - (Vector2)transform.position).magnitude;
        if (distanceTraveled >= maxDistance)
        {
            RemoveDagger();
        }
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
            else if (rb.isKinematic)
            {
                Debug.Log("Dagger hit a wall" + transform.position);
            }
            RemoveDagger();
        }
    }

    void RemoveDagger()
    {
        OnHit?.Invoke(transform.position);
        cam.RemoveFromView(transform);
        Destroy(gameObject);
    }
}