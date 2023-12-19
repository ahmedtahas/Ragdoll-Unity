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
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
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
        GameManager.Instance.OnPushEnemy -= HandlePushEnemy;
    }

    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
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
            IgnoreCollision(GetComponentsInChildren<Rigidbody2D>(), kate.GetComponentsInChildren<Rigidbody2D>());
            kate.GetComponent<CharacterManager>().Instantiate(Constants.KATE);
            kateMovement = kate.GetComponent<Movement>();
            skill.StartDuration(true);
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
