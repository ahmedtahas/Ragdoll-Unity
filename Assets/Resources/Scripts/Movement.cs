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
        BounceOnImpact[] bounceOnImpacts = GetComponentsInChildren<BounceOnImpact>();

        // Subscribe to their signals
        foreach (BounceOnImpact bounceOnImpact in bounceOnImpacts)
        {
            bounceOnImpact.OnBounce += HandleBounce;
        }
        rb = GetComponent<Rigidbody2D>();
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
        direction = movementStick.GetInput();
        if (direction.magnitude > 0.1f)
            rb.velocity = movementStick.GetInput() * speed;
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
