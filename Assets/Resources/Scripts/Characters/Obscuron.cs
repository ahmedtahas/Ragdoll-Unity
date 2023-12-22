using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obscuron : MonoBehaviour
{
    bool isOnCooldown = false;
    Skill skill;

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
            GameManager.Instance.BlindEnemy(skill.duration, gameObject);
        }
    }
}
