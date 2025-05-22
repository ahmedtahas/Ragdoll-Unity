using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject changeCharacterButton;
    public GameObject mainMenuButton;
    MultiTargetCamera multiTargetCamera;

    void OnEnable()
    {
        resumeButton.GetComponent<Button>().onClick.AddListener(ResumeGame);
        changeCharacterButton.GetComponent<Button>().onClick.AddListener(EndGame);
        mainMenuButton.GetComponent<Button>().onClick.AddListener(EndGame);
    }

    void OnDisable()
    {
        resumeButton.GetComponent<Button>().onClick.RemoveListener(ResumeGame);
        changeCharacterButton.GetComponent<Button>().onClick.RemoveListener(EndGame);
        mainMenuButton.GetComponent<Button>().onClick.RemoveListener(EndGame);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        GameManager.Instance.GetComponent<TimeController>().ResumeSlowDown();
    }

    public void EndGame()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] skills = GameObject.FindGameObjectsWithTag("Skill");
        GameObject spawnManager = GameObject.Find("SpawnManager");
        if (spawnManager != null)
        {
            Destroy(spawnManager);
        }
        foreach (GameObject player in players)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(player.transform.Find(Constants.HIP).transform);
            Destroy(player);
        }
        foreach (GameObject skill in skills)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(skill.transform);
            Destroy(skill);
        }
        ResumeGame();
    }

}
