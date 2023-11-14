using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    GameObject character;
    
    public void ShowCharacter(string characterName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().RemoveFromView(player.transform.Find(Constants.HIP).transform);
            Destroy(player);
        }
        character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
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
