using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    GameObject meteorPrefab;
    GameObject meteorInstance;
    CharacterManager characterManager;
    bool isOnCooldown = false;
    Skill skill;
    float damage;
    Transform body;

    void Start()
    {
        body = transform.Find(Constants.BODY);
        damage = GetComponent<CharacterManager>().characterDamage;
        meteorPrefab = Resources.Load<GameObject>(Constants.METEOR_PREFAB_PATH);
        characterManager = GetComponent<CharacterManager>();
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
            StartCoroutine(CreateMeteorAfterDelay());
        }
    }

    void HandleMeteorHit(bool isHit, bool isHeadshot)
    {
        if (isHit)
        {
            if (isHeadshot)
            {
                GameManager.Instance.DamageEnemy(damage * 3, gameObject);
            }
            else
            {
                GameManager.Instance.DamageEnemy(damage, gameObject);
            }
        }
        skill.EndDuration();
    }

    IEnumerator CreateMeteorAfterDelay()
    {
        isOnCooldown = true;
        skill.StartDuration(true);
        yield return new WaitForSeconds(0.5f);
        CreateMeteor();
    }

    public void CreateMeteor()
    {
        Vector3 position = GameManager.Instance.trapped ? new Vector3(transform.position.x * 0.5f, transform.position.y * 0.5f, 0) : transform.position;
        meteorInstance = Instantiate(meteorPrefab, position, Quaternion.identity);
        meteorInstance.GetComponent<Meteor>().OnHit += HandleMeteorHit;
        meteorInstance.GetComponent<Meteor>().FollowEnemy(gameObject, GameManager.Instance.enemy, skill.duration, damage);
    }
}
