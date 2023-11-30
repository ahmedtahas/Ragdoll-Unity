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
        transform.Find("UI").gameObject.SetActive(false);
        movement = transform.Find(Constants.BODY).GetComponent<Movement>();
        hip = transform.Find(Constants.HIP).gameObject;
        GameManager.Instance.enemy = hip;
        GameManager.Instance.OnFreezeEnemy += HandleFreezeEnemy;
        GameManager.Instance.OnBlindEnemy += HandleBlindEnemy;
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
        GameManager.Instance.OnTrapEnemy += HandleTrapEnemy;
        GameManager.Instance.OnDamageEnemy += HandleDamageEnemy;
        // StartCoroutine(Chase());
    }

    void HandleDamageEnemy(float damage)
    {
        health.TakeDamage(damage);
    }

    void HandleTrapEnemy(bool trapped)
    {
        // if (trapped)
        // {
        //     StartCoroutine(TrapEnemy());
        // }
        // else
        // {
        //     StopCoroutine(TrapEnemy());
        // }
    }

    void HandlePushEnemy(Vector2 direction, float force)
    {
        body.GetComponent<BounceOnImpact>().Pushed(direction, force);
    }

    void HandleBlindEnemy(float duration)
    {
        // StartCoroutine(BlindEnemy(duration));
    }

    void HandleFreezeEnemy(float duration)
    {
        // StartCoroutine(FreezeEnemy(duration));
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
