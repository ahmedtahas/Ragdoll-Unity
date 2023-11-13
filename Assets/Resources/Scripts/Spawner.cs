using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject character;
    public string characterName = "Holstar";
    public string enemyName = "Chronopen";

    void Start()
    {
        
        character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, transform.position, Quaternion.identity);
        character.GetComponent<CharacterManager>().Instantiate(characterName);
        character.transform.position = new Vector3(100, 0, 0);
        character.name = characterName;
        character = Instantiate(Resources.Load(Constants.CHARACTER_PREFAB_PATH) as GameObject, transform.position, Quaternion.identity);
        character.GetComponent<CharacterManager>().Instantiate(enemyName);
        character.transform.position = new Vector3(-100, 0, 0);
        character.name = enemyName;
        character.transform.Find("Body").GetComponent<Rigidbody2D>().isKinematic = true;
        character.transform.Find("Body").GetComponent<Movement>().enabled = false;
        character.transform.Find("UI").gameObject.SetActive(false);

    }
}
