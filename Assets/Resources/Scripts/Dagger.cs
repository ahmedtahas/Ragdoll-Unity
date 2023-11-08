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
    public int damageAmount = 10;
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
            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                if (collision.gameObject.CompareTag("Skill"))
                {
                    Debug.Log("Dagger hit a skill");
                }
                else if (collision.gameObject.CompareTag("Head"))
                {
                    collision.gameObject.GetComponentInParent<Health>().TakeDamage(damageAmount * 2);
                }
                else
                {
                    collision.gameObject.GetComponentInParent<Health>().TakeDamage(damageAmount);
                }
                
                // Teleport the player behind the enemy
                Vector2 enemyPosition = collision.transform.position;
                Vector2 behindEnemy = enemyPosition - (Vector2)transform.right;
            }
            else if (rb.bodyType == RigidbodyType2D.Static)
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