using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infrade : MonoBehaviour
{
    bool isOnCooldown = false;
    TimeController timeController;
    Damage[] damagers;
    float damage;
    Skill skill;

    void Start()
    {
        damage = GetComponent<CharacterManager>().characterDamage;
        timeController = GetComponent<TimeController>();
        damagers = GetComponentsInChildren<Damage>();
    }
    

    void OnEnable()
    {
        SkillStick skillStick = transform.GetComponentInChildren<SkillStick>();
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
    }

    void OnDisable()
    {
        SkillStick skillStick = transform.GetComponentInChildren<SkillStick>();
        if (skillStick != null)
        {
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
            skill.OnDurationEnd += HandleDurationEnd;
        }
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }

    void HandleSkillSignal()
    {
        if (!isOnCooldown)
        {
            skill.StartDuration(true);
            Rage();
        }
    }


    void HandleDurationEnd()
    {
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
            damage.SetDamage(this.damage * 2);
        }
    }
}
