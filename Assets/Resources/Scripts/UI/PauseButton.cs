using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PauseGame);
    }
    void OnDisable()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.RemoveListener(PauseGame);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
}
