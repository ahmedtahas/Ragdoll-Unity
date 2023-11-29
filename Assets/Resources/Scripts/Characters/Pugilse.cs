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

    void Start()
    {
        timeController = GetComponent<TimeController>();
        skill = GetComponent<Skill>();
        if (skill != null)
        {
            skill.CanUseSkill += HandleCooldown;
            skill.OnDurationEnd += HandleDurationEnd;
        }
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.Click);
            skillStick.OnClick += HandleSignal;
        }
        damagers = GetComponentsInChildren<Damage>();

        // Subscribe to their signals
        foreach (Damage damage in damagers)
        {
            damage.OnHit += HandleHit;
            this.damage = damage.damage;
        }
        maxCombo = (int)skill.duration;
        cooldown = skill.cooldown;
        skill.SetCooldownBar(0.0f, "0");
    }

    void HandleCooldown()
    {
        isOnCooldown = false;
    }

    void HandleSignal()
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
            damage.SetDamage(damage.damage * (combo + 1) / 2);
        }
    }


}
