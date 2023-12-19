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
    public CharacterSelectionDisplay characterDisplayScript;
    public TextMeshProUGUI selectedOptionText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;
    public GameObject spawner;

    private bool isDropdownOpen = false;

    void Start()
    {
        dropdownButton.onClick.AddListener(ToggleDropdown);
        ToggleDropdown();
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
        characterDisplayScript.ShowCharacter(characterDisplayScript.GetButton(selectedOptionText.text));
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
        GameManager.Instance.playerCharacter = selectedOptionText.text;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(player.transform.Find(Constants.HIP).transform);
            Destroy(player);
        }
        if (GameManager.Instance.gameMode == Constants.SINGLE_PLAYER)
        {
            GameObject character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, spawner.transform.position - new Vector3(100, 0, 0), Quaternion.identity);
            character.GetComponent<CharacterManager>().Instantiate(selectedOptionText.text);
            character.name = selectedOptionText.text;
            character.transform.Find(Constants.BODY).transform.position = new Vector3(-100, 0, 0);
            GameObject.Find("nwui").SetActive(false);
            character.transform.Find("UI").gameObject.SetActive(true);
            character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, spawner.transform.position + new Vector3(100, 0, 0), Quaternion.identity);
            character.GetComponent<CharacterManager>().Instantiate(Constants.BOT);
            character.name = Constants.BOT;
        }

    }
}
