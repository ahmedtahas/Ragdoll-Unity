using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tin : MonoBehaviour
{
    bool isOnCooldown = false;
    Skill skill;
    float knockback = 0.0f;
    float damage = 0.0f;
    float maximumCharge;
    float multiplier = 1.0f;
    float range = 20.0f;
    GameObject shockwavePrefab;
    GameObject shockwave;
    Coroutine growShockwaveCoroutine;
    Transform body;



    void Start()
    {
        body = transform.Find(Constants.BODY);
        shockwavePrefab = Resources.Load<GameObject>("Prefabs/Shockwave");
        maximumCharge = GetComponent<CharacterManager>().characterSkillDuration;
        knockback = GetComponent<CharacterManager>().characterKnockback;
        damage = GetComponent<CharacterManager>().characterDamage;
        damage = 2000.0f;
    }

    void OnEnable()
    {
        SkillStick skillStick = transform.GetComponentInChildren<SkillStick>();
        skill = GetComponent<Skill>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.ChargeUp);
            skillStick.OnChargeUp += HandleSkillSignal;
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
            skillStick.OnChargeUp -= HandleSkillSignal;
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

    void HandleSkillSignal(bool released, float chargeTime)
    {
        if (!isOnCooldown && !released)
        {
            transform.GetComponent<Movement>().enabled = false;
            skill.StartDuration(false);
            CreateShockwave();
        }
        if (!isOnCooldown && released)
        {
            isOnCooldown = true;
            skill.StartCooldown();
            transform.GetComponent<Movement>().enabled = true;
            DestroyShockwave();
            if (chargeTime >= maximumCharge)
            {
                multiplier += maximumCharge;
            }
            else
            {
                multiplier += chargeTime;
            }
            if ((GameManager.Instance.enemy.transform.position - GameManager.Instance.player.transform.position).magnitude <= range * multiplier)
            {
                GameManager.Instance.PushEnemy((Vector2)(GameManager.Instance.player.transform.position - GameManager.Instance.enemy.transform.position), knockback, gameObject);
                GameManager.Instance.DamageEnemy(damage * multiplier, gameObject);
            }
            multiplier = 1.0f;
        }
    }

    void CreateShockwave()
    {
        if (growShockwaveCoroutine != null)
        {
            StopCoroutine(growShockwaveCoroutine);
        }
        shockwave = Instantiate(shockwavePrefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        shockwave.transform.parent = GameManager.Instance.player.transform;
        growShockwaveCoroutine = StartCoroutine(GrowShockwave(shockwave, maximumCharge));
    }

    IEnumerator GrowShockwave(GameObject shockwave, float duration)
    {
        Vector3 initialScale = new Vector3(7, 7, 1);
        Vector3 finalScale = new Vector3(33, 33, 1);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalized time, goes from 0 to 1
            shockwave.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is exactly what we want
        shockwave.transform.localScale = finalScale;
    }

    void DestroyShockwave()
    {
        if (growShockwaveCoroutine != null)
        {
            StopCoroutine(growShockwaveCoroutine);
            growShockwaveCoroutine = null;
        }
        Destroy(shockwave);
    }
}
