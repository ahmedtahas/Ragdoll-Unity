using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInventoryDisplay : MonoBehaviour
{
    Button characterButtonPrefab;
    GameObject character;
    public GameObject characterDisplay;
    public ItemGrid itemGrid;
    
    void OnEnable()
    {
        characterButtonPrefab = Resources.Load<Button>("Prefabs/InventoryCharacterButton");
        PopulateCharacterList();
        if (character != null)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(character.transform.Find(Constants.HIP).transform);
            Destroy(character);
        }
    }

    void ClearCharacterList()
    {
        foreach (Transform child in characterDisplay.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void PopulateCharacterList()
    {
        ClearCharacterList();
        foreach (string characterName in Constants.CHARACTERS)
        {
            Button newCharacter = Instantiate(characterButtonPrefab, characterDisplay.transform);
            newCharacter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = characterName;
            newCharacter.onClick.AddListener(() => { ShowCharacter(newCharacter); });
            newCharacter.onClick.AddListener(() => { itemGrid.GetItems(newCharacter); });
        }
    }

    public void ShowCharacter(Button button)
    {
        string characterName = button.GetComponentInChildren<TextMeshProUGUI>().text;
        if (character != null)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(character.transform.Find(Constants.HIP).transform);
            Destroy(character);
        }
        
        character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        character.GetComponent<CharacterManager>().Instantiate(characterName);

        character.transform.Find(Constants.BODY).GetComponent<Rigidbody2D>().isKinematic = true;
        character.GetComponent<Movement>().enabled = false;
        character.transform.Find("UI").gameObject.SetActive(false);
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj.GetComponent<Rigidbody2D>() != null)
        {
            obj.layer = newLayer;
        }

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void OnDisable()
    {
        if (character != null)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(character.transform.Find(Constants.HIP).transform);
            Destroy(character);
        }
        character = null;
    }

    public Button GetButton(string characterName)
    {
        foreach (Transform child in transform.Find("../Characters/Viewport/Container"))
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>().text == characterName)
            {
                return child.GetComponent<Button>();
            }
        }
        return null;
    }

}
