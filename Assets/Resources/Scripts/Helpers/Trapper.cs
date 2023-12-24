using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : MonoBehaviour
{
    Transform body;
    GameObject smallRing;
    GameObject smallRingPrefab;
    CharacterManager characterManager;

    void OnEnable()
    {
        characterManager = GetComponent<CharacterManager>();
        smallRingPrefab = Resources.Load<GameObject>("Prefabs/SmallRing");
        body = transform.Find(Constants.BODY);
        GameManager.Instance.OnTrapEnemy += HandleTrapEnemy;
    }

    void OnDisable()
    {
        GameManager.Instance.OnTrapEnemy -= HandleTrapEnemy;
    }

    void HandleTrapEnemy(bool trapped)
    {
        if (trapped)
        {
            body.position = new Vector3(body.position.x * 0.5f, body.position.y * 0.5f, 0);
            body.position = GameManager.Instance.GetInternalPosition(body.position, characterManager.characterRadius);
            body.position = GameManager.Instance.GetAvailablePosition(gameObject, body.position);
            smallRing = Instantiate(smallRingPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Destroy(smallRing);
        }
        
    }

}
