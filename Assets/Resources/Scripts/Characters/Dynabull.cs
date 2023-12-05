using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynabull : MonoBehaviour
{
    Health health;
    bool isOnCooldown = false;
    bool isOnDuration = false;
    Skill skill;
    Transform rf;
    Transform lf;
    Transform body;

    

    void Start()
    {
        body = transform.Find(Constants.BODY);
        rf = transform.Find(Constants.RF);
        lf = transform.Find(Constants.LF);
        health = GetComponent<Health>();
        
    }

    void OnEnable()
    {
        health.OnDamageTaken += DamageTaken;
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
            skill.OnDurationEnd += HandleDurationEnd;
        }
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
    }

    void OnDisable()
    {
        health.OnDamageTaken -= DamageTaken;
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
            skill.OnDurationEnd -= HandleDurationEnd;
        }
        GameManager.Instance.OnPushEnemy -= HandlePushEnemy;
    }

    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
    }

    void DamageTaken(float amount)
    {
        if (isOnDuration) health.Heal(amount  * 0.5F);
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }

    void HandleDurationEnd()
    {
        ShrinkShields();
    }

    void HandleSkillSignal()
    {
        if (!isOnCooldown)
        {
            skill.StartDuration(true);
            GrowShields();
        }
    }

    void GrowShields()
    {
        isOnDuration = true;
        rf.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        lf.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    void ShrinkShields()
    {
        isOnDuration = false;
        rf.localScale = new Vector3(1f, 1f, 1f);
        lf.localScale = new Vector3(1f, 1f, 1f);
        isOnCooldown = true;
    }


}
