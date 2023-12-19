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
    bool hit = false;

    public event Action<Vector3, bool, bool> OnHit;

    void Start()
    {
        cam = GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>();
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
        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic && collision.gameObject.tag != "Skill")
       {
            hit = true;
            if (collision.gameObject.CompareTag("Head"))
            {
                OnHit?.Invoke(transform.position, true, true);
            }
            else
            {
                OnHit?.Invoke(transform.position, true, false);
            }
        }
        RemoveDagger();
    }

    void RemoveDagger()
    {
        if (!hit) OnHit?.Invoke(transform.position, false, false);
        cam.RemoveFromView(transform);
        Destroy(gameObject);
    }
}