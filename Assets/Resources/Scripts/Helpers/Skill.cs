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
    public float cooldown;
    float remainingCooldown;
    public float duration;
    float remainingDuration;
    bool duringCooldown;

    public event Action CanUseSkill;
    public event Action OnDurationEnd;

    Coroutine durationCoroutine;

    public Coroutine StartDuration(bool cooldown = true)
    {
        // Stop the previous duration coroutine if it's still running
        if (durationCoroutine != null)
        {
            StopCoroutine(durationCoroutine);
        }

        // Start a new duration coroutine and store it in durationCoroutine
        durationCoroutine = StartCoroutine(DurationRoutine(cooldown));

        return durationCoroutine;
    }

    public void StartCooldown()
    {
        // Stop the duration coroutine if it's still running
        if (durationCoroutine != null)
        {
            StopCoroutine(durationCoroutine);
            remainingDuration = duration;
            durationCoroutine = null;
        }

        StartCoroutine(CooldownRoutine());
    }

    public void SetCooldownBar(float fillAmount, string text)
    {
        cooldownBar.fillAmount = (fillAmount  * 0.5F) + 0.5f;
        cooldownText.text = text;
        remainingDuration = fillAmount * duration;
    }

    public void EndDuration()
    {
        remainingDuration = 0;
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
    IEnumerator DurationRoutine(bool cooldown)
    {
        if (duringCooldown)
            yield break;
        duringCooldown = true;
        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            cooldownBar.fillAmount = ((remainingDuration / duration)  * 0.5F) + 0.5f;
            cooldownText.text = remainingDuration.ToString("F1") + "s";
            yield return null;
        }
        remainingDuration = duration;
        OnDurationEnd?.Invoke();
        if (cooldown)
            StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        while (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            cooldownBar.fillAmount = ((1 - (remainingCooldown / cooldown))  * 0.5F) + 0.5f;
            cooldownText.text = remainingCooldown.ToString("F1") + "s";
            yield return null;
        }
        remainingCooldown = cooldown;
        cooldownBar.fillAmount = 1.0f;
        cooldownText.text = "Ready";
        CanUseSkill?.Invoke();
        duringCooldown = false;
    }
}
