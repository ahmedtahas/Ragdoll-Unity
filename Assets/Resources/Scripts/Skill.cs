using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using System;

public class Skill : NetworkBehaviour
{
    public SkillStick skillStick;
    public Image cooldownBar;
    public TextMeshProUGUI cooldownText;
    float cooldown;
    float remainingCooldown;
    public float duration;
    float remainingDuration;
    bool duringCooldown;

    public event Action<bool> CanUseSkill;

    void Start()
    {
        skillStick.OnAim += HandleAim;
        skillStick.OnChargeUp += HandleChargeUp;
        skillStick.OnClick += HandleClick;
    }

    void HandleAim(Vector2 direction, bool isReleased)
    {
        if (isReleased)
        {
            StartCoroutine(DurationRoutine(true));
        }
    }

    void HandleChargeUp(bool isReleased, float chargeTime)
    {
        if (isReleased)
        {
            StartCoroutine(CooldownRoutine());
        }
        else
        {
            StartCoroutine(DurationRoutine(false));
        }
    }

    void HandleClick()
    {
        StartCoroutine(DurationRoutine(true));
    }

    public void SetStats(float cooldown, float duration)
    {
        this.cooldown = cooldown;
        remainingCooldown = cooldown;
        this.duration = duration;
        remainingDuration = duration;
        cooldownBar.fillAmount = 1.0f;
        cooldownText.text = "Ready";
    }
    IEnumerator DurationRoutine(bool cooldown = true)
    {
        if (duringCooldown)
            yield break;
        duringCooldown = true;
        Debug.Log("DurationRoutine  " + duration + "  " + remainingDuration);
        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            cooldownBar.fillAmount = ((remainingDuration / duration) / 2) + 0.5f;
            cooldownText.text = remainingDuration.ToString("F1") + "s";
            yield return null;
        }
        remainingDuration = duration;
        if (cooldown)
            StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        while (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            cooldownBar.fillAmount = ((1 - (remainingCooldown / cooldown)) / 2) + 0.5f;
            cooldownText.text = remainingCooldown.ToString("F1") + "s";
            yield return null;
        }
        remainingCooldown = cooldown;
        cooldownBar.fillAmount = 1.0f;
        cooldownText.text = "Ready";
        CanUseSkill?.Invoke(true);
        duringCooldown = false;
    }
}
