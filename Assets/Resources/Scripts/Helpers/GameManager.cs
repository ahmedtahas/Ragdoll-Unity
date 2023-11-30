using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    
}
