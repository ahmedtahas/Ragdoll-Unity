using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    MovementStick movementStick;
    Rigidbody2D body;
    Vector2 direction;
    float speed = 1f;
    void Start()
    {
        movementStick = transform.Find("UI/MovementStick").GetComponent<MovementStick>();
        movementStick.OnMove += HandleMove;

        body = transform.Find(Constants.BODY).GetComponent<Rigidbody2D>();
    }
    public void HandleMove(Vector2 direction)
    {
        this.direction = direction;
    }

    void FixedUpdate()
    {
        if (direction.magnitude > 0.1f)
            body.AddForce(direction * speed);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

}
