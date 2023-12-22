using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyrobyte : MonoBehaviour
{
    bool isOnCooldown = false;
    Skill skill;
    Transform body;
    float speed;
    ParticleSystem freezeParticles;
    ParticleSystem freezeParticlesPrefab;

    void OnEnable()
    {
        body = transform.Find(Constants.BODY);
        freezeParticlesPrefab = Resources.Load<ParticleSystem>("Prefabs/FreezeParticles");
        ParticleSystem.MainModule freezeParticlesPrefabMain = freezeParticlesPrefab.main;
        speed = freezeParticlesPrefabMain.startSpeed.constant;
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
            GameManager.Instance.FreezeEnemy(body.position, skill.duration, gameObject);
        }
    }

}
