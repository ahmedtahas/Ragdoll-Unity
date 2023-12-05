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
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        skill = GetComponent<Skill>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.Click);
            // Subscribe to the OnSignalSent event
            skillStick.OnClick += HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill += HandleCooldown;
        }
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
    }

    void OnDisable()
    {
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            // Unsubscribe from the OnSignalSent event
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.CanUseSkill -= HandleCooldown;
        }
        GameManager.Instance.OnPushEnemy -= HandlePushEnemy;
    }

    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
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
        meteorInstance = Instantiate(meteorPrefab, transform.position, Quaternion.identity);
        meteorInstance.GetComponent<Meteor>().OnHit += HandleMeteorHit;
        meteorInstance.GetComponent<Meteor>().FollowEnemy(gameObject, GameManager.Instance.enemy, skill.duration, damage);
    }
}
