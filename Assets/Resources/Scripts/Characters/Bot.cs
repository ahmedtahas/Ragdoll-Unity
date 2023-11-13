using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bot : MonoBehaviour
{
    Movement movement;
    GameObject player;
    GameObject hip;
    void Start()
    {
        transform.Find("UI").gameObject.SetActive(false);
        movement = transform.Find(Constants.BODY).GetComponent<Movement>();
        hip = transform.Find(Constants.HIP).gameObject;
        GameManager.Instance.enemy = hip;
        StartCoroutine(Chase());
    }


    IEnumerator Chase()
    {
        while (player == null)
        {
            player = GameManager.Instance.player;
            yield return new WaitForSeconds(0.1f);
        }
        while (true)
        {
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
