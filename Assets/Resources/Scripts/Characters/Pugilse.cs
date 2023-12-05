using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugilse : MonoBehaviour
{
    Rigidbody2D rfRigidbody;
    bool isOnCooldown = false;
    TimeController timeController;
    int combo = 0;
    Damage[] damagers;
    int maxCombo;
    float cooldown;
    float damage;
    Skill skill;
    Transform body;

    void Start()
    {
        body = transform.Find(Constants.BODY);
        damage = GetComponent<CharacterManager>().characterDamage;
        timeController = GetComponent<TimeController>();
        maxCombo = (int)skill.duration;
        cooldown = skill.cooldown;
        skill.SetCooldownBar(0.0f, "0");
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
            skill.OnDurationEnd += HandleDurationEnd;
        }
        damagers = GetComponentsInChildren<Damage>();
        foreach (Damage damage in damagers)
        {
            damage.OnHit += HandleHit;
        }
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
    }

    void OnDisable()
    {
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            // Unsubscribe from the OnSignalSent event
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
        }
        foreach (Damage damage in damagers)
        {
            damage.OnHit -= HandleHit;
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
        if (!isOnCooldown && combo > 0)
        {
            skill.StartDuration(true);
            Rage();
        }
    }

    void HandleHit()
    {
        if (combo < maxCombo)
        {
            combo++;
            skill.SetCooldownBar((float)combo / maxCombo, combo.ToString());
        }
    }

    void HandleDurationEnd()
    {
        combo = 0;
        GetComponent<Skill>().SetStats(cooldown, (float)maxCombo);
        foreach (Damage damage in damagers)
        {
            damage.SetDamage(this.damage);
        }
    }

    void Rage()
    {
        isOnCooldown = true;
        timeController.SlowDownTime();
        foreach (Damage damage in damagers)
        {
            damage.SetDamage(damage.damage * (combo + 1)  * 0.5F);
        }
    }


}
