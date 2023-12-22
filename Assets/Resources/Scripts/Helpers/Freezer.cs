using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Freezer : MonoBehaviour
{
    Rigidbody2D[] rigidbodies;
    Transform body;
    float speed;
    ParticleSystem freezeParticlesPrefab;


    void Start()
    {   
        freezeParticlesPrefab = Resources.Load<ParticleSystem>("Prefabs/FreezeParticles");
        ParticleSystem.MainModule freezeParticlesPrefabMain = freezeParticlesPrefab.main;
        speed = freezeParticlesPrefabMain.startSpeed.constant;
        body = transform.Find(Constants.BODY);
        rigidbodies = GetComponentsInChildren<Rigidbody2D>();
    }

    void OnEnable()
    {
        GameManager.Instance.OnFreezeEnemy += HandleFreezeEnemy;
    }
    void OnDisable()
    {
        GameManager.Instance.OnFreezeEnemy -= HandleFreezeEnemy;
    }
    void HandleFreezeEnemy(Vector3 position, float duration, GameObject source)
    {
        if (source != gameObject) StartCoroutine(FreezeSelf(position, duration));
    }
    IEnumerator FreezeSelf(Vector3 position, float duration)
    {
        float distance = Vector3.Distance(position, body.position);
        float delay = distance / speed;
        yield return new WaitForSeconds(delay);
        GetComponent<Movement>().enabled = false;
        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        yield return new WaitForSeconds(duration);
        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }
        GetComponent<Movement>().enabled = true;
    }
}
