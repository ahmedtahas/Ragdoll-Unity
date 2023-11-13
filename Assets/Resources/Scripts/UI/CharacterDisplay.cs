using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    GameObject character;
    public void ShowCharacter(string characterName)
    {
        int layer = LayerMask.NameToLayer(Constants.CHARACTER_DISPLAY_LAYER);
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                if (obj.transform.Find(Constants.HIP) != null)
                    GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(obj.transform.Find(Constants.HIP).transform);
                Destroy(obj);
            }
        }
        character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        character.GetComponent<CharacterManager>().Instantiate(characterName);

        character.transform.Find(Constants.BODY).GetComponent<Rigidbody2D>().isKinematic = true;
        character.transform.Find(Constants.BODY).GetComponent<Movement>().enabled = false;
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


}
