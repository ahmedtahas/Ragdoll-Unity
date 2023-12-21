using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cyrobyte : MonoBehaviour
{
    bool isOnCooldown = false;
    Skill skill;
    Transform body;
    float speed = 200.0f;
    ParticleSystem freezeParticles;
    ParticleSystem freezeParticlesPrefab;

    void OnEnable()
    {
        body = transform.Find(Constants.BODY);
        freezeParticlesPrefab = Resources.Load<ParticleSystem>("Prefabs/FreezeParticles");
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
            isOnCooldown = true;
            skill.StartDuration(true);
            freezeParticles = Instantiate(freezeParticlesPrefab, body.position, Quaternion.identity);
            float distance = Vector3.Distance(GameManager.Instance.player.transform.position, GameManager.Instance.enemy.transform.position);
            float delay = distance / speed;
            StartCoroutine(FreezeAfterDelay(delay));
        }
    }

    IEnumerator FreezeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.FreezeEnemy(skill.duration, gameObject);
    }

}
