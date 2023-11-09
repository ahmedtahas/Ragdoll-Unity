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
        meteorPrefab = Resources.Load<GameObject>("Prefabs/Meteor");
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
            CreateMeteor();
        }
    }

    void HandleMeteorHit()
    {
        skill.EndDuration();
    }

    public void CreateMeteor()
    {
        isOnCooldown = true;
        meteorInstance = Instantiate(meteorPrefab, transform.position, Quaternion.identity);
        meteorInstance.GetComponent<Meteor>().OnHit += HandleMeteorHit;
        meteorInstance.GetComponent<Meteor>().FollowEnemy(characterManager.GetEnemy(gameObject).Find("Body/Stomach/Hip").gameObject, skill.duration, damage.damage);
    }
}
