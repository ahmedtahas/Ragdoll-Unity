using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    public MovementStick movementStick;
    Rigidbody2D rb;
    Vector2 direction;
    float speed = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        direction = movementStick.GetInput();
        if (direction.magnitude > 0.1f)
            rb.velocity = movementStick.GetInput() * speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
