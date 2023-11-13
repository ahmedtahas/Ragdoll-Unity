using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System;

public class Stele : MonoBehaviour
{
    MultiTargetCamera multiTargetCamera;
    GameObject afterImage;
    private Transform barrel;
    private Transform body;
    private Transform hip;
    private GameObject daggerPrefab;
    public float throwForce = 0.5f;
    public float spinSpeed = 5f;
    GameObject indicatorPrefab;
    GameObject indicator;
    int hitCount = 0;
    int maxHitCount = 2;
    bool aiming = false;
    bool isOnCooldown = false;
    Damage[] damages;

    void Start()
    {
        afterImage = Resources.Load<GameObject>("Prefabs/SteleAfterImage");
        multiTargetCamera = GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>();
        hip = transform.Find(Constants.HIP);
        indicatorPrefab = Resources.Load("Prefabs/Indicator") as GameObject;
        body = transform.Find(Constants.BODY);
        barrel = transform.Find("Body/RUA/RLA");
        daggerPrefab = Resources.Load(Constants.DAGGER_PREFAB_PATH) as GameObject;
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
        damages = GetComponentsInChildren<Damage>();

        // Subscribe to their signals
        foreach (Damage damage in damages)
        {
            damage.OnHit += HandleHit;
        }
    }
    void HandleHit()
    {
        if (hitCount < maxHitCount && !isOnCooldown)
        {
            hitCount++;
        }
    }

    void Update()
    {
        if (aiming) indicator.transform.position = hip.position;
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }
    
    void HandleSkillSignal(Vector2 direction, bool isReleased)
    {
        if (isReleased && !isOnCooldown && hitCount == maxHitCount)
        {
            hitCount = 0;
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
        GameObject afterImageInstance = Instantiate(afterImage, body.position, Quaternion.identity);
        multiTargetCamera.AddToView(afterImageInstance.transform);

        float distance = Vector3.Distance(body.position, position);

        float waitTime = 0.25f + 0.25f * (distance / 100f);
        
        body.GetComponent<Renderer>().enabled = false;
        foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        StartCoroutine(RemoveAfterImage(afterImageInstance, waitTime));
        body.position = position;
    }

    private IEnumerator RemoveAfterImage(GameObject afterImageInstance, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        body.GetComponent<Renderer>().enabled = true;
        foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }
        multiTargetCamera.RemoveFromView(afterImageInstance.transform);

        Destroy(afterImageInstance);
    }

}
