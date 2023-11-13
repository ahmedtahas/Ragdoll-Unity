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
        
        body = transform.Find("Body");
        DisableJoints();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            health.TakeDamage(10);
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
            SaveState();
            StartCoroutine(ChronopenRoutine());
        }
    }

    private IEnumerator ChronopenRoutine()
    {
        yield return new WaitForSeconds(skill.duration);
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
        body.transform.position = GetComponent<CharacterManager>().GetAvailablePosition(gameObject, savedPosition);
        health.TakeDamage((-(savedHealth - health.currentHealth) * 2) * 0.69f);
    }
}