using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronopen : MonoBehaviour
{
    private Transform body;
    private Vector3 savedPosition;
    private float savedHealth;
    private Health health;
    bool isOnCooldown = false;
    Skill skill;

    void Start()
    {
        health = GetComponent<Health>();
        body = transform.Find(Constants.BODY);
        DisableJoints();
    }

    void OnEnable()
    {
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        skill = GetComponent<Skill>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.Click);
            skillStick.OnClick += HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.OnDurationEnd += HandleDurationEnd;
            skill.CanUseSkill += HandleCooldown;
        }
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
    }

    void OnDisable()
    {
        SkillStick skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.OnClick -= HandleSkillSignal;
        }
        if (skill != null)
        {
            skill.OnDurationEnd -= HandleDurationEnd;
            skill.CanUseSkill -= HandleCooldown;
        }
        GameManager.Instance.OnPushEnemy -= HandlePushEnemy;
    }

    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
    }
    public void DisableJoints()
    {
        // List of paths to the child objects
        string[] paths = new string[]
        {
            Constants.RF,
            Constants.LF,
        };

        foreach (string path in paths)
        {
            // Find the child object
            Transform child = transform.Find(path);

            if (child != null)
            {
                // Get all the SpringJoint2D components in the child object
                SpringJoint2D[] joints = child.GetComponentsInChildren<SpringJoint2D>();

                // Disable each joint
                foreach (SpringJoint2D joint in joints)
                {
                    joint.enabled = false;
                }
            }
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
            skill.StartDuration(true);
            SaveState();
        }
    }


    void HandleDurationEnd()
    {
        RestoreState();
    }

    private void SaveState()
    {
        isOnCooldown = true;
        savedPosition = body.transform.position;
        savedHealth = health.currentHealth;
    }

    private void RestoreState()
    {
        body.transform.position = GameManager.Instance.GetAvailablePosition(gameObject, savedPosition);
        health.Heal(((savedHealth - health.currentHealth) * 2) * 0.69f);
    }
}