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
    bool hitCooldown = false;
    void Start()
    {
        movementStick.OnMove += HandleMove;
        BounceOnImpact[] bounceOnImpacts = GetComponentsInChildren<BounceOnImpact>();

        // Subscribe to their signals
        foreach (BounceOnImpact bounceOnImpact in bounceOnImpacts)
        {
            bounceOnImpact.OnBounce += HandleBounce;
        }
        rb = GetComponent<Rigidbody2D>();
    }
    public void HandleMove(Vector2 direction)
    {
        this.direction = direction;
    }

    private void HandleBounce()
    {
        hitCooldown = true;
        StartCoroutine(OnBounceCooldown());
    }
    void FixedUpdate()
    {
        if (hitCooldown)
            return;
        if (direction.magnitude > 0.1f)
            rb.velocity = direction * speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private IEnumerator OnBounceCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        hitCooldown = false;
    }
}
