using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDown : MonoBehaviour
{
    public GameObject dropdownMenu;
    public Button dropdownButton;
    public TextMeshProUGUI selectedOptionText;

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
        selectedOptionText.text = optionButton.GetComponentInChildren<Text>().text;
        ToggleDropdown();
    }
}
