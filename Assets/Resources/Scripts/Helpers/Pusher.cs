using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    Transform body;
    void OnEnable()
    {
        body = transform.Find(Constants.BODY);
        GameManager.Instance.OnPushEnemy += HandlePushEnemy;
    }
    void OnDisable()
    {
        GameManager.Instance.OnPushEnemy -= HandlePushEnemy;
    }
    void HandlePushEnemy(Vector2 direction, float force, GameObject source)
    {
        if (source != gameObject) body.GetComponent<BounceOnImpact>().Pushed(direction, force);
    }
}
