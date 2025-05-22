using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Health : MonoBehaviour
{
    Image healthBar;
    TextMeshProUGUI healthText;
    SkillStick skillStick;
    Image enemyHealthBar;
    TimeController timeController;
    public float maxHealth;
    public float currentHealth;
    private LayerMask deathLayer;
    public Transform[] childTransforms;

    [HideInInspector] public GameObject lastDamagedPart; // Store the last part that took damage

    public event Action<float, GameObject> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action<bool> OnEnemyDeath;

    void OnEnable()
    {

        skillStick = transform.Find("UI/SkillStick").GetComponent<SkillStick>();
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
            healthBar.fillAmount = ((currentHealth / maxHealth) * 0.5F) + 0.5f;
        }
        OnHealthChanged?.Invoke(healthBar.fillAmount, gameObject);
    }

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (source == null || source == gameObject)
        {
            return;
        }

        // Store the collision object as the last damaged part
        if (source.GetComponent<Damage>() != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(source.transform.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.transform.IsChildOf(transform) &&
                    (collider.CompareTag("Damagable") || collider.CompareTag("Head")))
                {
                    lastDamagedPart = collider.gameObject;
                    break;
                }
            }
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
            healthBar.fillAmount = ((currentHealth / maxHealth) * 0.5F) + 0.5f;
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

        timeController = GameManager.Instance.GetComponent<TimeController>();
        // timeController = GetComponent<TimeController>();
        Debug.Log("Death routine started");
        timeController.SlowDownTime(0.1f, 3f);

        foreach (Transform child in childTransforms)
        {

            child.gameObject.layer = deathLayer;
        }
        if (lastDamagedPart != null)
        {
            // Disable joints in the damaged part
            foreach (var joint in lastDamagedPart.GetComponentsInChildren<Joint2D>())
            {
                joint.enabled = false;
            }

            // Change layer of damaged part
            lastDamagedPart.layer = deathLayer;

            // Disable bounce and damage components
            BounceOnImpact bounce = lastDamagedPart.GetComponent<BounceOnImpact>();
            if (bounce != null) bounce.enabled = false;

            Damage damageComponent = lastDamagedPart.GetComponent<Damage>();
            if (damageComponent != null) damageComponent.enabled = false;

            // Make the damaged part flash one last time in a darker red
            DamageFlash bodyFlash = transform.Find("Body").GetComponent<DamageFlash>();
            if (bodyFlash != null)
            {
                bodyFlash.FlashBodyPart(lastDamagedPart, 1.5f); // More intense flash
            }
        }

        // Wait for 1 second before disabling the rest of the joints
        yield return new WaitForSeconds(0.2f);

        // Now disable all remaining joints and change layers
        foreach (Transform child in childTransforms)
        {
            // Skip the already disabled part
            if (lastDamagedPart != null && child.gameObject == lastDamagedPart)
                continue;

            // Disable joints
            foreach (var joint in child.GetComponentsInChildren<Joint2D>())
            {
                joint.enabled = false;
            }

            // Change layer

            // Disable components
            BounceOnImpact bounce = child.GetComponent<BounceOnImpact>();
            if (bounce != null) bounce.enabled = false;

            Damage damageComponent = child.GetComponent<Damage>();
            if (damageComponent != null) damageComponent.enabled = false;
        }

        // Wait a bit more before disabling character functionality
        yield return new WaitForSeconds(2f);

        Die();

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    private void Die()
    {
        Debug.Log("Die method called");
        GetComponent<Health>().enabled = false;
        GetComponent<CharacterManager>().enabled = false;
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
            skillStick.enabled = false;
            SpawnManager spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            spawnManager.RespawnBot();
            OnEnemyDeath?.Invoke(false);
        }
        if (health == 1 && gameObject.name != Constants.BOT)
        {
            skillStick.enabled = true;
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}