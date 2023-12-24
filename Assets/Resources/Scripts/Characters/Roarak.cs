using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        StartCoroutine(Shrink());
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
            StartCoroutine(Grow());
        }
    }

    IEnumerator Grow()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        float initialRadius = characterManager.characterRadius;

        foreach (Damage damage in damagers)
        {
            damage.SetDamage(this.damage * multiplier);
        }
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 2f; 

            transform.localScale = Vector3.Lerp(initialScale, initialScale * multiplier, t);
            characterManager.characterRadius = Mathf.Lerp(initialRadius, initialRadius * multiplier, t);


            yield return null;
        }
        transform.localScale = initialScale * multiplier;
        characterManager.characterRadius = initialRadius * multiplier;
    }

    IEnumerator Shrink()
    {
        float elapsedTime = 0f;

        foreach (Damage damage in damagers)
        {
            damage.SetDamage(this.damage);
        }
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 1f; 

            transform.localScale = Vector3.Lerp(transform.localScale, scale, t);
            characterManager.characterRadius = Mathf.Lerp(characterManager.characterRadius, radius, t);


            yield return null;
        }
        transform.localScale = scale;
        characterManager.characterRadius = radius;
    }
}
