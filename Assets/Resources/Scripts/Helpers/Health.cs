using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthText;
    public float maxHealth;
    public float currentHealth;
    private LayerMask deathLayer;
    public Transform[] childTransforms;

    void Start()
    {
        deathLayer = LayerMask.NameToLayer("Dead");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthText.text = currentHealth.ToString();
        healthBar.fillAmount = ((currentHealth / maxHealth) / 2) + 0.5f;

        if (currentHealth <= 0)
        {
            healthText.text = "0";
            healthBar.fillAmount = 0.5f;
            StartCoroutine(DeathRoutine());
        }
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
        GameObject.Find("MultiTargetCamera").GetComponent<MultiTargetCamera>().RemoveFromView(transform.Find("Body/Stomach/Hip").transform);
        // Destroy(gameObject);
    }

    public void SetHealth(float health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        healthBar.fillAmount = 1;
        healthText.text = currentHealth.ToString();
    }
}