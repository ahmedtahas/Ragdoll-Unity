using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplica : MonoBehaviour
{
    GameObject kate;
    GameObject katePrefab;
    Movement kateMovement;
    // Start is called before the first frame update
    MultiTargetCamera multiTargetCamera;
    private Transform body;
    Skill skill;
    bool isOnCooldown = false;

    void Start()
    {
        katePrefab = Resources.Load<GameObject>("Prefabs/Character") as GameObject;
        multiTargetCamera = GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>();
        body = transform.Find(Constants.BODY);
    }

    void OnEnable()
    {
        SkillStick skillStick = transform.GetComponentInChildren<SkillStick>();
        skill = GetComponent<Skill>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.AimAndRelease);
            skillStick.OnAim += HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill += HandleCooldown;
            skill.OnDurationEnd += HandleDurationEnd;
        }
    }


    void OnDisable()
    {
        SkillStick skillStick = transform.GetComponentInChildren<SkillStick>();
        if (skillStick != null)
        {
            skillStick.OnAim -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
        }
    }

    
    void HandleDurationEnd()
    {
        isOnCooldown = true;
        if (kate != null)
        {
            multiTargetCamera.RemoveFromView(kate.transform.Find(Constants.HIP).transform);
            Destroy(kate);
            kate = null;
        }
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }
    
    void HandleSkillSignal(Vector2 direction, bool isReleased)
    {
        if (isOnCooldown) return;

        if (kate == null)
        {
            kate = Instantiate(katePrefab, body.position, Quaternion.identity);
            kate.transform.rotation = body.rotation;
            IgnoreCollision(GetComponentsInChildren<Rigidbody2D>(), kate.GetComponentsInChildren<Rigidbody2D>());
            kate.GetComponent<CharacterManager>().Instantiate(Constants.KATE);
            Rigidbody2D[] rigidbodies = kate.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rb in rigidbodies)
            {
                rb.tag = "Skill";
                rb.GetComponent<BounceOnImpact>().SetSelf(gameObject);
            }
            Damage[] damages = kate.GetComponentsInChildren<Damage>();
            foreach (Damage damage in damages)
            {
                damage.SetSelf(gameObject);
            }
            kateMovement = kate.GetComponent<Movement>();
            skill.StartDuration(true);
        }
        else if (isReleased)
        {
            kateMovement.HandleMove(Vector2.zero);
        }
        else
        {
            kateMovement.HandleMove(direction);
        }
    }

    void IgnoreCollision(Rigidbody2D[] drbs, Rigidbody2D[] krbs)
    {
        foreach (Rigidbody2D drb in drbs)
        {
            foreach (Rigidbody2D krb in krbs)
            {
                Physics2D.IgnoreCollision(drb.GetComponent<Collider2D>(), krb.GetComponent<Collider2D>());
            }
        }
    }

}
