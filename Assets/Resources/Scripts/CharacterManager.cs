using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour
{
    
    private bool usesWeapon;
    private bool isTwoHanded;
    private Vector3 characterScale;
    private GameObject rf;
    private GameObject lf;
    
    public void Instantiate(string character)
    {
        rf = transform.Find("Body/RUA/RLA/RF").gameObject;
        lf = transform.Find("Body/LUA/LLA/LF").gameObject;
        switch (character)
        {
            case "Chronopen":
                characterScale = new Vector3(1.0f, 1.0f, 1.0f);
                isTwoHanded = true;
                usesWeapon = true;
                break;
            case "Holstar":
                characterScale = new Vector3(1.0f, 1.0f, 1.0f);
                isTwoHanded = false;
                usesWeapon = true;
                break;
            case "Stele":
                characterScale = new Vector3(0.8f, 0.8f, 0.8f);
                isTwoHanded = true;
                usesWeapon = true;
                break;
        }
        transform.localScale = characterScale;
        if (usesWeapon)
        {
            rf.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character + "/RF");
            rf.GetComponent<WeaponCollision>().UpdateCollisionShape();
            if (isTwoHanded)
            {
                lf.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character + "/LF");
                lf.GetComponent<WeaponCollision>().UpdateCollisionShape();
            }
        }
        System.Type type = System.Type.GetType(character);
        if (type != null)
        {
            gameObject.AddComponent(type);
        }
        else
        {
            Debug.LogError("Script: " + character + " not found.");
        }
    }
}
