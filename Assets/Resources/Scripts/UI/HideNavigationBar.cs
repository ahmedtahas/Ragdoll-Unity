using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideNavigationBar : MonoBehaviour
{
    public Button showButton;

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterSeconds(3));
    }

    IEnumerator DeactivateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        showButton.gameObject.SetActive(true);
    }
}
