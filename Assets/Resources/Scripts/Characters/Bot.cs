using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bot : MonoBehaviour
{
    Movement movement;
    GameObject player;
    GameObject hip;
    Health health;
    Transform body;
    void Start()
    {
        body = transform.Find(Constants.BODY);
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();
        hip = transform.Find(Constants.HIP).gameObject;
        GameManager.Instance.enemy = hip;
        // StartCoroutine(Chase());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.TrapEnemy(true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.TrapEnemy(false);
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnBlindEnemy += HandleBlindEnemy;
        GameManager.Instance.OnDamageEnemy += HandleDamageEnemy;
    }
    void OnDisable()
    {
        GameManager.Instance.OnBlindEnemy -= HandleBlindEnemy;
        GameManager.Instance.OnDamageEnemy -= HandleDamageEnemy;
    }

    void HandleDamageEnemy(float damage, GameObject source)
    {
        if (source != gameObject) health.TakeDamage(damage);
    }

    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
    }

    void HandleBlindEnemy(float duration, GameObject source)
    {
        // StartCoroutine(BlindEnemy(duration));
    }

    IEnumerator Chase()
    {
        yield return new WaitUntil(() => GameManager.Instance.player != null);
        player = GameManager.Instance.player;
        while (true)
        {
            if (health.currentHealth <= 0)
            {
                break;
            }
            if ((player.transform.position - hip.transform.position).magnitude > 30.0f)
            {
                yield return new WaitForSeconds(1.0f);
                movement.HandleMove((player.transform.position - hip.transform.position).normalized);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
                movement.HandleMove((hip.transform.position - player.transform.position).normalized);
            }
        }
    }
}
