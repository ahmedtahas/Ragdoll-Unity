using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stele : MonoBehaviour
{
    private Transform barrel;
    private Transform body;
    private GameObject daggerPrefab;

    public float throwForce = 0.5f;
    public float spinSpeed = 5f;

    void Start()
    {
        body = transform.Find("Body");
        barrel = transform.Find("Body/RUA/RLA/RF");
        daggerPrefab = Resources.Load("Prefabs/Dagger") as GameObject;
        SkillStick skillStick = GameObject.Find("SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.AimAndRelease);
            // Subscribe to the OnSignalSent event
            skillStick.OnAim += HandleSignal;
        }
    }
    
    void HandleSignal(Vector2 signal, bool isReleased)
    {
        // Handle the signal
        Debug.Log("Received signal: " + signal + ", isReleased: " + isReleased);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ThrowDagger();
        }
    }

    void ThrowDagger()
    {
        GameObject dagger = Instantiate(daggerPrefab, barrel.position, barrel.rotation) as GameObject;
        Rigidbody2D rb = dagger.GetComponent<Rigidbody2D>();
        rb.AddForce(barrel.right * throwForce);
        rb.angularVelocity = spinSpeed;

        // Get all the Collider2D components of the dagger
        Collider2D[] daggerColliders = dagger.GetComponents<Collider2D>();

        // Get all the Collider2D components of all the player's children
        Collider2D[] playerChildrenColliders = GetComponentsInChildren<Collider2D>();

        // Make each dagger collider ignore collision with each child collider
        foreach (Collider2D daggerCollider in daggerColliders)
        {
            foreach (Collider2D childCollider in playerChildrenColliders)
            {
                Physics2D.IgnoreCollision(daggerCollider, childCollider);
            }
        }
    }
    public void Teleport(Vector3 position)
    {
        body.transform.position = position;
    }
}
