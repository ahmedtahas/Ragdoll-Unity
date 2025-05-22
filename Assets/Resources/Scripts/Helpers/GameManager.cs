using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    
    public Vector2 roomPositiveDimensions = new Vector2(160.0f, 80.0f);
    public Vector2 roomNegativeDimensions = new Vector2(-160.0f, -80.0f);
    public bool trapped = false;
    public GameObject player;
    private GameObject _enemy;
    public GameObject enemy
    {
        get { return _enemy; }
        set
        {
            _enemy = value;
            StartCoroutine(EnemySet());
        }
    }
    public string gameMode;
    public string playerCharacter;
    public string enemyCharacter;
    public Transform playerTransform;
    public Transform enemyTransform;
    public float playerDamage;
    public float enemyDamage;
    public float playerHealth;
    public float enemyHealth;
    public Health playerHealthComponent;
    public Health enemyHealthComponent;
    public float playerKnockback;
    public float enemyKnockback;
    public event Action<float, GameObject> OnEnemyHealthChanged;
    public event Action<Vector3, float, GameObject> OnFreezeEnemy;
    public event Action<Vector2, float, GameObject> OnPushEnemy;
    public event Action<float, GameObject> OnBlindEnemy;
    public event Action<bool> OnTrapEnemy;
    public event Action<float, GameObject> OnDamageEnemy;
    GameObject spawnManager;
    GameObject spawnManagerPrefab;

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
    }

    public void DamageEnemy(float damage, GameObject caller = null)
    {
        OnDamageEnemy?.Invoke(damage, caller);
    }

    public void TrapEnemy(bool trapped)
    {
        this.trapped = trapped;
        OnTrapEnemy?.Invoke(trapped);
    }

    public void BlindEnemy(float duration, GameObject caller = null)
    {
        OnBlindEnemy?.Invoke(duration, caller);
    }

    public void PushEnemy(Vector2 direction, float force, GameObject caller = null)
    {
        OnPushEnemy?.Invoke(direction, force, caller);
    }

    public void FreezeEnemy(Vector3 position, float duration, GameObject caller = null)
    {
        OnFreezeEnemy?.Invoke(position, duration, caller);
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
        gameMode = mode;
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

    public Vector3 GetInternalPosition(Vector3 callerEndPosition, float callerRadius)
    {
        if (!trapped)
        {
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
        }
        else
        {
            if (callerEndPosition.x > roomPositiveDimensions.x * 0.5f)
            {
                callerEndPosition.x = roomPositiveDimensions.x * 0.5f - (callerRadius  * 0.5F);
            }
            else if (callerEndPosition.x < roomNegativeDimensions.x * 0.5f)
            {
                callerEndPosition.x = roomNegativeDimensions.x * 0.5f + (callerRadius  * 0.5F);
            }
            if (callerEndPosition.y > roomPositiveDimensions.y * 0.5f)
            {
                callerEndPosition.y = roomPositiveDimensions.y * 0.5f - (callerRadius  * 0.5F);
            }
            else if (callerEndPosition.y < roomNegativeDimensions.y * 0.5f)
            {
                callerEndPosition.y = roomNegativeDimensions.y * 0.5f + (callerRadius  * 0.5F);
            }
        }
        return callerEndPosition;
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
        callerEndPosition = GetInternalPosition(callerEndPosition, callerRadius);
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
