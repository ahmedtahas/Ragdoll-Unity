using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    MovementStick movementStick;
    Rigidbody2D rb;
    Vector2 direction;
    float speed = 1f;
    void Start()
    {
        // if (!IsOwner)
        // {
        //     Debug.Log("Not owner");
        //     return;
        // }
        movementStick = GameObject.Find("MovementStick").GetComponent<MovementStick>();
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        // if (!IsOwner)
        // {
        //     Debug.Log("Not owner");
        //     return;
        // }
        direction = movementStick.GetInput();
        if (direction.magnitude > 0.1f)
            rb.velocity = movementStick.GetInput() * speed;
    }
}
