using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Freezer : MonoBehaviour
{
    Rigidbody2D[] rigidbodies;
    void OnEnable()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody2D>();
        GameManager.Instance.OnFreezeEnemy += HandleFreezeEnemy;
    }
    void OnDisable()
    {
        GameManager.Instance.OnFreezeEnemy -= HandleFreezeEnemy;
    }
    void HandleFreezeEnemy(float duration, GameObject source)
    {
        if (source != gameObject) StartCoroutine(FreezeSelf(duration));
    }
    IEnumerator FreezeSelf(float duration)
    {
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
