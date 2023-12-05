using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    
    public Vector2 roomPositiveDimensions = new Vector2(130.0f, 60.0f);
    public Vector2 roomNegativeDimensions = new Vector2(-130.0f, -60.0f);

    public GameObject player;
    public GameObject enemy;
    public string playerCharacter;
    public string enemyCharacter;
    public Transform playerTransform;
    public Transform enemyTransform;
    public float playerDamage;
    public float enemyDamage;
    public float playerHealth;
    public float enemyHealth;
    public float playerKnockback;
    public float enemyKnockback;
    public event Action<float, GameObject> OnEnemyHealthChanged;
    public event Action<float, GameObject> OnFreezeEnemy;
    public event Action<Vector2, float, GameObject> OnPushEnemy;
    public event Action<float, GameObject> OnBlindEnemy;
    public event Action<bool, GameObject> OnTrapEnemy;
    public event Action<float, GameObject> OnDamageEnemy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(EnemySet());
    }

    public void DamageEnemy(float damage, GameObject caller = null)
    {
        OnDamageEnemy?.Invoke(damage, caller);
    }

    public void TrapEnemy(bool trapped, GameObject caller = null)
    {
        OnTrapEnemy?.Invoke(trapped, caller);
    }

    public void BlindEnemy(float duration, GameObject caller = null)
    {
        OnBlindEnemy?.Invoke(duration, caller);
    }

    public void PushEnemy(Vector2 direction, float force, GameObject caller = null)
    {
        OnPushEnemy?.Invoke(direction, force, caller);
    }

    public void FreezeEnemy(float duration, GameObject caller = null)
    {
        OnFreezeEnemy?.Invoke(duration, caller);
    }

    

    IEnumerator EnemySet()
    {
        yield return new WaitUntil(() => enemy != null);
        enemyTransform.GetComponent<Health>().OnHealthChanged += HandleEnemyHealthChanged;
    }

    private void HandleEnemyHealthChanged(float health, GameObject caller = null)
    {
        OnEnemyHealthChanged?.Invoke((health - 0.5f)  * 2, caller);
    }

    public void SetMode(string mode)
    {
        PlayerPrefs.SetString(Constants.GAME_MODE, mode);
    }

    public Vector2 RotateVector(Vector2 vector, float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(angleInRadians);
        float sinTheta = Mathf.Sin(angleInRadians);
        return new Vector2(
            vector.x * cosTheta - vector.y * sinTheta,
            vector.x * sinTheta + vector.y * cosTheta
        );
    }
    

    public Vector3 GetAvailablePosition(GameObject caller, Vector3 position)
    {
        float callerRadius = caller.GetComponent<CharacterManager>().characterRadius;
        Vector3 callerHipPosition = caller.transform.Find(Constants.HIP).position;
        Vector3 callerEndPosition = position;
        float callerVectorLenght = (callerEndPosition - callerHipPosition).magnitude;
        Vector3 callerVector = (callerEndPosition - callerHipPosition).normalized;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Dictionary<GameObject, (float radius, Vector3 hipPosition)> playerData = new Dictionary<GameObject, (float, Vector3)>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == caller)
            {
                continue;
            }
            CharacterManager characterManager = players[i].GetComponent<CharacterManager>();
            Transform hip = players[i].transform.Find(Constants.HIP);

            if (characterManager != null && hip != null)
            {
                playerData[players[i]] = (characterManager.characterRadius, hip.position);
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = player.Value.radius + callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght += range * 1.5f;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght += range;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        if (callerEndPosition.x > roomPositiveDimensions.x)
        {
            callerEndPosition.x = roomPositiveDimensions.x - (callerRadius  * 0.5F);
        }
        else if (callerEndPosition.x < roomNegativeDimensions.x)
        {
            callerEndPosition.x = roomNegativeDimensions.x + (callerRadius  * 0.5F);
        }
        if (callerEndPosition.y > roomPositiveDimensions.y)
        {
            callerEndPosition.y = roomPositiveDimensions.y - (callerRadius  * 0.5F);
        }
        else if (callerEndPosition.y < roomNegativeDimensions.y)
        {
            callerEndPosition.y = roomNegativeDimensions.y + (callerRadius  * 0.5F);
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght -= range * 1.5f;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght -= range;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        return callerEndPosition;
    }
}
