using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holstar : MonoBehaviour
{
    private Transform rf;
    private Vector2 barrelPosition;
    Vector2 barrelVector;
    private GameObject bulletPrefab;
    private Rigidbody2D rfRigidbody;
    public float recoilForce = 42.0f;
    bool isOnCooldown = false;
    Skill skill;
    TimeController timeController;
    float damage;
    float force;
    Transform body;

    void Start()
    {
        body = transform.Find(Constants.BODY);
        damage = GetComponent<CharacterManager>().characterDamage;
        force = GetComponent<CharacterManager>().characterKnockback;
        timeController = GetComponent<TimeController>();
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        rf = transform.Find(Constants.RF);
        rfRigidbody = rf.GetComponent<Rigidbody2D>();
        barrelVector = (rf.GetComponent<WeaponCollision>().GetUpperRightCorner() - Vector2.zero);
    }


    void OnEnable()
    {
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        skill = GetComponent<Skill>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.Click);
            skillStick.OnClick += HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill += HandleCooldown;
        }
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
    }

    void OnDisable()
    {
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
        }
        GameManager.Instance.OnPushEnemy -= HandlePushEnemy;
    }

    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }

    void HandleSkillSignal()
    {
        if (!isOnCooldown)
        {
            skill.StartCooldown();
            Shoot();
        }
    }

    void HandleHit(Vector3 hitPosition, bool isHeadshot)
    {
        if (isHeadshot)
        {
            GameManager.Instance.DamageEnemy(damage * 3, gameObject);
        }
        else
        {
            GameManager.Instance.DamageEnemy(damage, gameObject);
        }
        GameManager.Instance.PushEnemy((new Vector2(hitPosition.x, hitPosition.y) - new Vector2(rf.transform.position.x, rf.transform.position.y)).normalized, force, gameObject);
    }

    void Shoot()
    {
        timeController.SlowDownTime();
        isOnCooldown = true;
        barrelPosition = (Vector2)rf.position + GameManager.Instance.RotateVector(barrelVector, rf.eulerAngles.z);

        GameObject bullet = Instantiate(bulletPrefab, barrelPosition, rf.rotation) as GameObject;
        bullet.GetComponent<Bullet>().OnHit += HandleHit;
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
