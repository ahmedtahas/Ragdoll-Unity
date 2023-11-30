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
    public event Action<float> OnEnemyHealthChanged;
    public event Action<float> OnFreezeEnemy;
    public event Action<Vector2, float> OnPushEnemy;
    public event Action<float> OnBlindEnemy;
    public event Action<bool> OnTrapEnemy;
    public event Action<float> OnDamageEnemy;

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

    public void DamageEnemy(float damage)
    {
        OnDamageEnemy?.Invoke(damage);
    }

    public void TrapEnemy(bool trapped)
    {
        OnTrapEnemy?.Invoke(trapped);
    }

    public void BlindEnemy(float duration)
    {
        OnBlindEnemy?.Invoke(duration);
    }

    public void PushEnemy(Vector2 direction, float force)
    {
        OnPushEnemy?.Invoke(direction, force);
    }

    public void FreezeEnemy(float duration)
    {
        OnFreezeEnemy?.Invoke(duration);
    }

    

    IEnumerator EnemySet()
    {
        yield return new WaitUntil(() => enemy != null);
        enemyTransform.GetComponent<Health>().OnHealthChanged += HandleEnemyHealthChanged;
    }

    private void HandleEnemyHealthChanged(float health)
    {
        OnEnemyHealthChanged?.Invoke((health - 0.5f) * 2);
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
        Debug.Log(caller.name + " " + position + "  HEBELEEEE " + callerHipPosition + "  " + callerEndPosition + "  " + callerVector);
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
                Debug.Log("1  " + callerVectorLenght);
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
                Debug.Log(callerEndPosition);
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght += range;
                Debug.Log("2  " + callerVectorLenght);
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
                Debug.Log(callerEndPosition);
            }
        }
        Debug.Log(callerEndPosition);
        if (callerEndPosition.x > roomPositiveDimensions.x)
        {
            callerEndPosition.x = roomPositiveDimensions.x - callerRadius / 2;
        }
        else if (callerEndPosition.x < roomNegativeDimensions.x)
        {
            callerEndPosition.x = roomNegativeDimensions.x + callerRadius / 2;
        }
        if (callerEndPosition.y > roomPositiveDimensions.y)
        {
            callerEndPosition.y = roomPositiveDimensions.y - callerRadius / 2;
        }
        else if (callerEndPosition.y < roomNegativeDimensions.y)
        {
            callerEndPosition.y = roomNegativeDimensions.y + callerRadius / 2;
        }
        Debug.Log(callerEndPosition);
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght -= range * 1.5f;
                Debug.Log("3  " + callerVectorLenght);
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
                Debug.Log(callerEndPosition);
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght -= range;
                Debug.Log("4  " + callerVectorLenght);
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
                Debug.Log(callerEndPosition);
            }
        }
        return callerEndPosition;
    }
}
