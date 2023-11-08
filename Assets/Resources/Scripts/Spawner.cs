using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject character;
    public string characterName = "Holstar";

    void Start()
    {
        
        character = Instantiate(Resources.Load("Prefabs/Character") as GameObject, transform.position, Quaternion.identity);
        character.GetComponent<CharacterManager>().Instantiate(characterName);
        character.transform.position = new Vector3(100, 0, 0);

    }
}
