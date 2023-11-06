using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private LayerMask deathLayer;
    public Transform[] childTransforms;

    void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        deathLayer = LayerMask.NameToLayer("Dead");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
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
        GameObject.Find("MultiTargetCamera").GetComponent<MultiTargetCamera>().RemoveFromView(transform.Find("Body").transform);
        // Destroy(gameObject);
    }
}