using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject character;

    void Start()
    {
        
        character = Instantiate(Resources.Load("Prefabs/Character") as GameObject, transform.position, Quaternion.identity);
        character.GetComponent<CharacterManager>().Instantiate("Chronopen");

    }
}
