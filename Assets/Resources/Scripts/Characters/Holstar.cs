using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holstar : MonoBehaviour
{
    private Transform rf;
    private Vector2 barrelPosition;
    private GameObject bulletPrefab;
    private Rigidbody2D rfRigidbody;
    public float recoilForce = 100f;
    bool isOnCooldown = false;
    Skill skill;
    TimeController timeController;

    void Start()
    {
        timeController = GetComponent<TimeController>();
        skill = GetComponent<Skill>();
        if (skill != null)
        {
            skill.CanUseSkill += HandleCooldown;
        }
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.Click);
            skillStick.OnClick += HandleSignal;
        }
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        rf = transform.Find(Constants.RF);
        rfRigidbody = rf.GetComponent<Rigidbody2D>();
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }

    void HandleSignal()
    {
        if (!isOnCooldown)
        {
            skill.StartCooldown();
            Shoot();
        }
    }

    void Shoot()
    {
        timeController.SlowDownTime();
        isOnCooldown = true;
        barrelPosition = rf.GetComponent<WeaponCollision>().GetUpperRightCorner();
        
        GameObject bullet = Instantiate(bulletPrefab, barrelPosition, rf.rotation) as GameObject;
        // Get the Collider2D component of the bullet
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();

        // Get the Collider2D components of all the player's children
        Collider2D[] playerChildrenColliders = GetComponentsInChildren<Collider2D>();

        // Make the bullet ignore collision with all the player's children
        foreach (Collider2D childCollider in playerChildrenColliders)
        {
            Physics2D.IgnoreCollision(bulletCollider, childCollider);
        }
        
        rfRigidbody.AddForce(-rf.right * recoilForce, ForceMode2D.Impulse);
    }
}
