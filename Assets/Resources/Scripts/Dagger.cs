using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public float maxDistance = 100f;
    private Vector3 startPosition;
    private Stele player;

    void Start()
    {
        startPosition = transform.position;
        player = FindObjectOfType<Stele>();
    }

    void Update()
    {
        float distanceTraveled = ((Vector2)startPosition - (Vector2)transform.position).magnitude;
        if (distanceTraveled >= maxDistance)
        {
            Debug.Log("Dagger reached max distance");
            player.Teleport(transform.position);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                Debug.Log("Dagger hit an enemy");
                // Teleport the player behind the enemy
                Vector2 enemyPosition = collision.transform.position;
                Vector2 behindEnemy = enemyPosition - (Vector2)transform.right;
                player.Teleport(behindEnemy);
            }
            else if (rb.bodyType == RigidbodyType2D.Static)
            {
                Debug.Log("Dagger hit a wall" + transform.position);
                // Teleport the player to the dagger's position
                player.Teleport(transform.position);
            }
            Destroy(gameObject);
        }
    }
}