using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dropdown : MonoBehaviour
{
    public GameObject dropdownMenu;
    public Button dropdownButton;
    public GameObject characterDisplay;
    public CharacterDisplay characterDisplayScript;
    public TextMeshProUGUI selectedOptionText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;

    private bool isDropdownOpen = false;

    void Start()
    {
        dropdownButton.onClick.AddListener(ToggleDropdown);
    }

    public void ToggleDropdown()
    {
        isDropdownOpen = !isDropdownOpen;
        dropdownMenu.SetActive(isDropdownOpen);
    }

    public void SelectOption(Button optionButton)
    {
        selectedOptionText.text = optionButton.GetComponentInChildren<TextMeshProUGUI>().text;
        characterDisplay.SetActive(true);
        characterDisplayScript.ShowCharacter(selectedOptionText.text);
        healthText.text = Constants.CHARACTER_HEALTH_POINTS[selectedOptionText.text].ToString();
        damageText.text = Constants.CHARACTER_DAMAGES[selectedOptionText.text].ToString();
        speedText.text = Constants.CHARACTER_SPEEDS[selectedOptionText.text].ToString();
        levelText.text = "1";
        skillNameText.text = Constants.CHARACTER_SKILL_NAMES[selectedOptionText.text];
        skillDescriptionText.text = Constants.CHARACTER_SKILL_DESCRIPTIONS[selectedOptionText.text];

        ToggleDropdown();
    }

    public void CharacterSelected()
    {
        PlayerPrefs.SetString(Constants.SELECTED_CHARACTER, selectedOptionText.text);
        int layer = LayerMask.NameToLayer(Constants.CHARACTER_DISPLAY_LAYER);
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> objectsToDestroy = new List<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                objectsToDestroy.Add(obj);

            }
        }
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj.transform.Find(Constants.HIP) != null)
                GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(obj.transform.Find(Constants.HIP).transform);
            Destroy(obj);
        }

    }
}
