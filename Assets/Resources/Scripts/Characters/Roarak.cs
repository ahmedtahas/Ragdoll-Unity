using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roarak : MonoBehaviour
{
    Health health;
    bool isOnCooldown = false;
    bool isOnDuration = false;
    Skill skill;
    float multiplier = 1.5f;
    Damage[] damagers;
    float damage;
    Vector3 scale;
    float radius;
    CharacterManager characterManager;
    

    void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        damagers = GetComponentsInChildren<Damage>();
        damage = GetComponent<CharacterManager>().characterDamage;
        scale = transform.localScale;
        radius = characterManager.characterRadius;
    }

    void OnEnable()
    {
        health = GetComponent<Health>();
        health.OnDamageTaken += DamageTaken;
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
        health.OnDamageTaken -= DamageTaken;
        SkillStick skillStick = transform.GetComponentInChildren<SkillStick>();
        if (skillStick != null)
        {
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
            skill.OnDurationEnd -= HandleDurationEnd;
        }

    }


    void DamageTaken(float amount)
    {
        if (isOnDuration) health.Heal(amount  * 0.25F);
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }

    void HandleDurationEnd()
    {
        isOnDuration = false;
        Shrink();
        GameManager.Instance.TrapEnemy(false);
    }

    void HandleSkillSignal()
    {
        if (!isOnCooldown)
        {
            GameManager.Instance.TrapEnemy(true);
            isOnDuration = true;
            isOnCooldown = true;
            skill.StartDuration(true);
            Grow();
        }
    }

    void Grow()
    {
        transform.localScale *= multiplier;
        characterManager.characterRadius *= multiplier;
        foreach (Damage damage in damagers)
        {
            damage.SetDamage(this.damage * multiplier);
        }
    }

    void Shrink()
    {
        transform.localScale = scale;
        characterManager.characterRadius = radius;
        foreach (Damage damage in damagers)
        {
            damage.SetDamage(this.damage);
        }
    }
}
