using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Color flashColor = new Color(1f, 0.3f, 0.3f, 1f); // Default reddish color

    // Dictionary to track original colors for each body part
    private Dictionary<string, Color> originalColors = new Dictionary<string, Color>();
    private Dictionary<string, Coroutine> activeFlashes = new Dictionary<string, Coroutine>();

    // Flash a specific body part with default red color
    public void FlashBodyPart(GameObject bodyPart, float intensity = 1.0f)
    {
        FlashBodyPart(bodyPart, intensity, flashColor);
    }

    // Flash a specific body part with custom color
    public void FlashBodyPart(GameObject bodyPart, float intensity, Color customFlashColor)
    {
        if (bodyPart == null) return;

        // Get the sprite renderer of the body part
        SpriteRenderer spriteRenderer = bodyPart.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        // Generate a unique key for this body part
        string partKey = bodyPart.GetInstanceID().ToString();

        // Store the original color if we haven't already
        if (!originalColors.ContainsKey(partKey))
        {
            originalColors[partKey] = spriteRenderer.color;
        }

        // Stop any existing flash for this part
        if (activeFlashes.ContainsKey(partKey))
        {
            StopCoroutine(activeFlashes[partKey]);
            activeFlashes.Remove(partKey);
        }

        // Start a new flash for this part with the specified color
        Coroutine flashCoroutine = StartCoroutine(FlashPartRoutine(spriteRenderer, partKey, intensity, customFlashColor));
        activeFlashes[partKey] = flashCoroutine;
    }

    private IEnumerator FlashPartRoutine(SpriteRenderer spriteRenderer, string partKey, float intensity, Color customFlashColor)
    {
        // Make sure we have the reference to the renderer and original color
        if (spriteRenderer == null || !originalColors.ContainsKey(partKey))
        {
            yield break;
        }

        // Get original color
        Color originalColor = originalColors[partKey];

        // Adjust the flash color based on intensity
        Color targetColor = Color.Lerp(originalColor, customFlashColor, intensity);

        // Set color to the flash color
        spriteRenderer.color = targetColor;

        // Wait for flash duration
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            // Check if the sprite renderer still exists
            if (spriteRenderer == null)
            {
                yield break;
            }

            // Gradually return to original color
            float t = elapsed / flashDuration;
            spriteRenderer.color = Color.Lerp(targetColor, originalColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure we end with the original color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Remove from active flashes
        if (activeFlashes.ContainsKey(partKey))
        {
            activeFlashes.Remove(partKey);
        }
    }

    // This ensures all parts return to their original colors if the script is disabled
    private void OnDisable()
    {
        RestoreAllColors();
    }

    // This ensures colors are restored when the object is destroyed
    private void OnDestroy()
    {
        RestoreAllColors();
    }

    // Reset all parts to their original colors
    public void RestoreAllColors()
    {
        // Stop all active coroutines
        foreach (var flash in activeFlashes.Values)
        {
            if (flash != null)
            {
                StopCoroutine(flash);
            }
        }
        activeFlashes.Clear();

        // Restore all original colors
        foreach (var entry in originalColors)
        {
            // Find the sprite renderer by instance ID
            GameObject obj = FindObjectByInstanceID(int.Parse(entry.Key));
            if (obj != null)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = entry.Value;
                }
            }
        }
    }

    // Helper method to find objects by instance ID
    private GameObject FindObjectByInstanceID(int instanceID)
    {
        // Find all sprite renderers in children
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer.gameObject.GetInstanceID() == instanceID)
            {
                return renderer.gameObject;
            }
        }
        return null;
    }

    // Add a public method to force restore a specific part's color
    public void RestoreColor(GameObject bodyPart)
    {
        if (bodyPart == null) return;

        string partKey = bodyPart.GetInstanceID().ToString();
        if (originalColors.ContainsKey(partKey))
        {
            SpriteRenderer renderer = bodyPart.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = originalColors[partKey];
            }

            // Stop any active flash
            if (activeFlashes.ContainsKey(partKey))
            {
                StopCoroutine(activeFlashes[partKey]);
                activeFlashes.Remove(partKey);
            }
        }
    }
}