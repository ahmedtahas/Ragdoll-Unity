using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Health : MonoBehaviour
{
    Image healthBar;
    TextMeshProUGUI healthText;
    Image enemyHealthBar;
    public float maxHealth;
    public float currentHealth;
    private LayerMask deathLayer;
    public Transform[] childTransforms;

    public event Action<float, GameObject> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action<bool> OnEnemyDeath;

    void OnEnable()
    {
        if (healthBar == null)
        {
            healthBar = transform.Find("UI/Bars/Health/HBG/HFG").GetComponent<Image>();
        }
        if (healthText == null)
        {
            healthText = transform.Find("UI/Bars/HT").GetComponent<TextMeshProUGUI>();
        }
        if (enemyHealthBar == null)
        {
            enemyHealthBar = transform.Find("UI/EnemyHealthBG/EnemyHealth").GetComponent<Image>();
        }
        if (deathLayer == 0)
        {
            deathLayer = LayerMask.NameToLayer("Dead");
        }
        GameManager.Instance.OnDamageEnemy += TakeDamage;
        if (GameManager.Instance.gameMode == Constants.SINGLE_PLAYER)
        {
            GameManager.Instance.OnEnemyHealthChanged += SetBotHealth;
        }
        else
        {
            GameManager.Instance.OnEnemyHealthChanged += SetEnemyHealth;
        }
    }

    void OnDisable()
    {
        GameManager.Instance.OnDamageEnemy -= TakeDamage;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthText != null)
        {
            healthText.text = currentHealth % 1 == 0 ? currentHealth.ToString() : currentHealth.ToString("F1");
            healthBar.fillAmount = ((currentHealth / maxHealth)  * 0.5F) + 0.5f;
        }
        OnHealthChanged?.Invoke(healthBar.fillAmount, gameObject);
    }

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (source == null || source == gameObject)
        {
            return;
        }
        currentHealth -= amount;
        OnDamageTaken?.Invoke(amount);
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthText != null)
        {
            healthText.text = currentHealth % 1 == 0 ? currentHealth.ToString() : currentHealth.ToString("F1");
            healthBar.fillAmount = ((currentHealth / maxHealth)  * 0.5F) + 0.5f;
        }
        if (currentHealth <= 0)
        {
            if (healthText != null)
            {
                healthText.text = "0";
                healthBar.fillAmount = 0.5f;
            }
            StartCoroutine(DeathRoutine());
        }
        OnHealthChanged?.Invoke(healthBar.fillAmount, gameObject);
    }

    private IEnumerator DeathRoutine()
    {
        // Disable all joints in child objects
        foreach (var joint in GetComponentsInChildren<Joint2D>())
        {
            joint.enabled = false;
        }

        // Change collision layer of all child objects
        foreach (Transform child in childTransforms)
        {
            child.gameObject.layer = deathLayer;
            child.GetComponent<BounceOnImpact>().enabled = false;
            Damage damageComponent = child.GetComponent<Damage>();
            if (damageComponent != null)
            {
                damageComponent.enabled = false;
            }
        }

        yield return new WaitForSeconds(3);

        Die();

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    private void Die()
    {
        GetComponent<Health>().enabled = false;
        GetComponent<CharacterManager>().enabled = false;
        GetComponent<TimeController>().enabled = false;
        GetComponent<Pusher>().enabled = false;
        GetComponent<Freezer>().enabled = false;
        GetComponent<Trapper>().enabled = false;
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(transform.Find(Constants.HIP).transform);
    }

    public void SetHealth(float health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        healthBar.fillAmount = 1;
        healthText.text = currentHealth.ToString();
    }

    public void SetBotHealth(float health, GameObject bot = null)
    {
        if (bot == null || enemyHealthBar == null)
        {
            return;
        }
        enemyHealthBar.fillAmount = health;
        if (health <= 0 && gameObject.name != Constants.BOT)
        {
            GameManager.Instance.RespawnBot();
            OnEnemyDeath?.Invoke(false);
        }
    }

    public void SetEnemyHealth(float health, GameObject enemy = null)
    {
        if (enemy == null || enemy == gameObject)
        {
            return;
        }
        if (enemyHealthBar == null)
        {
            return;
        }
        enemyHealthBar.fillAmount = health;
        if (health <= 0)
        {
            OnEnemyDeath?.Invoke(true);
        }
    }
}