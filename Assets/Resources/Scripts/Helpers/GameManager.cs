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
}
