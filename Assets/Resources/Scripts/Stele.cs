using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class Stele : MonoBehaviour
{
    private Transform barrel;
    private Transform body;
    private Transform hip;
    private GameObject daggerPrefab;
    public float throwForce = 0.5f;
    public float spinSpeed = 5f;
    GameObject indicatorPrefab;
    GameObject indicator;
    bool aiming = false;
    bool isOnCooldown = false;

    void Start()
    {
        hip = transform.Find("Body/Stomach/Hip");
        indicatorPrefab = Resources.Load("Prefabs/Indicator") as GameObject;
        body = transform.Find("Body");
        barrel = transform.Find("Body/RUA/RLA");
        daggerPrefab = Resources.Load("Prefabs/Dagger") as GameObject;
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        Skill skill = GetComponent<Skill>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.AimAndRelease);
            // Subscribe to the OnSignalSent event
            skillStick.OnAim += HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill += HandleCooldown;
        }
    }

    void Update()
    {
        if (aiming) indicator.transform.position = hip.position;
    }

    void HandleCooldown(bool canUseSkill)
    {
        isOnCooldown = !canUseSkill;
    }
    
    void HandleSkillSignal(Vector2 direction, bool isReleased)
    {
        if (isReleased && !isOnCooldown)
        {
            aiming = false;
            Destroy(indicator);
            ThrowDagger(direction);
        }
        else if (!isReleased && !isOnCooldown)
        {
            aiming = true;
            if (indicator == null)
            {
                indicator = Instantiate(indicatorPrefab, hip.position, Quaternion.identity) as GameObject;
            }
            indicator.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }


    void ThrowDagger(Vector2 direction)
    {
        isOnCooldown = true;
        GameObject dagger = Instantiate(daggerPrefab, barrel.position, barrel.rotation) as GameObject;
        dagger.GetComponent<Dagger>().OnHit += HandleDaggerHit;
        Rigidbody2D rb = dagger.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * throwForce);

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

    void HandleDaggerHit(Vector3 position)
    {
        Teleport(GetComponent<CharacterManager>().GetAvailablePosition(gameObject, position));
    }

    public void Teleport(Vector3 position)
    {
        body.position = position;
        SpringJoint2D[] joints = GetComponentsInChildren<SpringJoint2D>();

        foreach (SpringJoint2D joint in joints)
        {
            joint.distance = 0.005f;
        }
    }

}
