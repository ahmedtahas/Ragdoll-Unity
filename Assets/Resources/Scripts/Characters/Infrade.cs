using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infrade : MonoBehaviour
{
    bool isOnCooldown = false;
    TimeController timeController;
    GameObject flameParticlePrefab;
    Transform rf;
    Damage[] damagers;
    float damage;
    Skill skill;
    private GameObject activeFlameParticle;

    void Start()
    {
        damage = GetComponent<CharacterManager>().characterDamage;
        timeController = GameManager.Instance.GetComponent<TimeController>();
        damagers = GetComponentsInChildren<Damage>();
        flameParticlePrefab = Resources.Load("Prefabs/FlameParticle") as GameObject;
        rf = transform.Find(Constants.RF);
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
            if (flameParticlePrefab != null)
            {
                Debug.Log("Blast particle prefab is not null");
                float mediumOffset = 0f;
                if (Constants.CHARACTER_PART_SIZES.TryGetValue("MEDIUM", out mediumOffset))
                {
                    mediumOffset = mediumOffset * 0.01f;
                    // Destroy any previous flame particle if it exists
                    if (activeFlameParticle != null)
                    {
                        Destroy(activeFlameParticle);
                    }

                    // Instantiate as a child of rf so it follows the hand
                    activeFlameParticle = Instantiate(
                        flameParticlePrefab,
                        rf
                    );
                    // Set local position offset
                    activeFlameParticle.transform.localPosition = new Vector3(0, mediumOffset, 0);
                    activeFlameParticle.transform.localRotation = Quaternion.identity;

                    // Set sorting layer and order
                    var psRenderer = activeFlameParticle.GetComponent<ParticleSystemRenderer>();
                    if (psRenderer != null)
                    {
                        psRenderer.sortingLayerName = "Foreground";
                        psRenderer.sortingOrder = 10;
                    }
                }
                else
                {
                    Debug.LogWarning("MEDIUM size not found in CHARACTER_PART_SIZES!");
                }
            }
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
        if (activeFlameParticle != null)
        {
            Destroy(activeFlameParticle);
            activeFlameParticle = null;
        }
    }

    void Rage()
    {
        isOnCooldown = true;
        timeController.SlowDownTime(0.2f, 1.0f);
        foreach (Damage damage in damagers)
        {
            damage.SetDamage(this.damage * 2);
        }
    }
}
