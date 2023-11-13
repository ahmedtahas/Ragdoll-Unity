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
    Damage damage;

    void Start()
    {
        damage = GetComponent<Damage>();
        meteorPrefab = Resources.Load<GameObject>(Constants.METEOR_PREFAB_PATH);
        characterManager = GetComponent<CharacterManager>();
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
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

    void HandleMeteorHit()
    {
        skill.EndDuration();
    }

    IEnumerator CreateMeteorAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        CreateMeteor();
    }

    public void CreateMeteor()
    {
        isOnCooldown = true;
        meteorInstance = Instantiate(meteorPrefab, transform.position, Quaternion.identity);
        meteorInstance.GetComponent<Meteor>().OnHit += HandleMeteorHit;
        meteorInstance.GetComponent<Meteor>().FollowEnemy(GameManager.Instance.enemy, skill.duration, damage.damage);
    }
}
