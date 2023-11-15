using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Health : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public Image enemyHealthBar;
    public float maxHealth;
    public float currentHealth;
    private LayerMask deathLayer;
    public Transform[] childTransforms;

    public event Action<float> OnHealthChanged;

    void Start()
    {
        deathLayer = LayerMask.NameToLayer("Dead");
        StartCoroutine(SetEnemyHealth());
    }

    IEnumerator SetEnemyHealth()
    {
        yield return new WaitUntil(() => GameManager.Instance.enemy != null);
        GameManager.Instance.OnEnemyHealthChanged += SetEnemyHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
            healthBar.fillAmount = ((currentHealth / maxHealth) / 2) + 0.5f;
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
        OnHealthChanged?.Invoke(healthBar.fillAmount);
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
        }

        yield return new WaitForSeconds(3);

        Die();
    }

    private void Die()
    {
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(transform.Find(Constants.HIP).transform);
    }

    public void SetHealth(float health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        healthBar.fillAmount = 1;
        healthText.text = currentHealth.ToString();
    }

    public void SetEnemyHealth(float health)
    {
        if (enemyHealthBar == null)
        {
            return;
        }
        enemyHealthBar.fillAmount = health;
    }
}