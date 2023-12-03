using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ItemGrid : MonoBehaviour
{
    // Assuming you have a reference to your Scroll Rect and Grid Layout Group
    public ScrollRect scrollRect;
    public GridLayoutGroup gridLayoutGroup;
    public CharacterInventoryDisplay characterDisplay;
    Button itemPrefab;
    string character;
    

    void Start()
    {
        itemPrefab = Resources.Load<Button>("Prefabs/Item");
        UpdateContentSize();
    }

    public void UpdateContentSize()
    {
        float totalHeight = gridLayoutGroup.cellSize.y * (int)Math.Ceiling(gridLayoutGroup.transform.childCount / 5.0f);
        totalHeight += gridLayoutGroup.spacing.y * ((int)Math.Ceiling(gridLayoutGroup.transform.childCount / 5.0f) - 1);
        totalHeight += gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        gridLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(gridLayoutGroup.GetComponent<RectTransform>().sizeDelta.x, totalHeight);
        scrollRect.content = gridLayoutGroup.GetComponent<RectTransform>();
    }

    public void GetItems(Button characterButton)
    {
        character = characterButton.GetComponentInChildren<TextMeshProUGUI>().text;
        ClearItems();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Skins");
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name.StartsWith(character + "_Display"))
            {
                Button newItem = Instantiate(itemPrefab, gridLayoutGroup.transform);
                newItem.GetComponent<Image>().sprite = sprite;
                newItem.GetComponent<Button>().onClick.AddListener(() => { SelectItem(newItem); });
            }
        }
    }

    public void SelectItem(Button item)
    {
        string skinCategory = item.GetComponent<Image>().sprite.name;
        int lastIndex = skinCategory.LastIndexOf('_');
        if (lastIndex > 0)
        {
            int secondToLastIndex = skinCategory.LastIndexOf('_', lastIndex - 1);
            if (secondToLastIndex >= 0 && lastIndex - secondToLastIndex > 1)
            {
                int skinIndex = int.Parse(skinCategory.Substring(secondToLastIndex + 1, lastIndex - secondToLastIndex - 1));
                skinCategory = skinCategory.Substring(lastIndex + 1);
                PlayerPrefs.SetInt(character + skinCategory + "Skin", skinIndex);
                characterDisplay.ShowCharacter(characterDisplay.GetButton(character));
            }
            else
            {
                Debug.LogError("Invalid sprite name: " + skinCategory);
            }
        }
        else
        {
            Debug.LogError("Invalid sprite name: " + skinCategory);
        }
    }

    public void ClearItems()
    {
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SortItems(Button sorter)
    {
        ClearItems();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Skins");
        string startSubstring = character + "_Display";
        string endSubstring = sorter.gameObject.name;
        if (endSubstring == "All")
        {
            endSubstring = "";
        }
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name.StartsWith(startSubstring) && sprite.name.EndsWith(endSubstring))
            {
                Button newItem = Instantiate(itemPrefab, gridLayoutGroup.transform);
                newItem.GetComponent<Image>().sprite = sprite;
                newItem.GetComponent<Button>().onClick.AddListener(() => { SelectItem(newItem); });
            }
        }
    }
    

}
