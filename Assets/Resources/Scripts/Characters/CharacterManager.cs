using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : MonoBehaviour
{
    private Vector2 roomPositiveDimensions = new Vector2(130.0f, 55.0f);
    private Vector2 roomNegativeDimensions = new Vector2(-130.0f, -55.0f);
    private bool usesWeapon = false;
    private bool isTwoHanded = false;
    private Vector3 characterScale = new Vector3(1.0f, 1.0f, 1.0f);
    public float characterRadius = 50.0f;
    private float characterSpeed = 1.0f;
    private float characterHealth = 100.0f;
    private float characterCooldown = 10.0f;
    private float characterDamage = 10.0f;
    private float characterKnockback = 5.0f;
    private float characterSkillDuration = 10.0f;
    private GameObject rf;
    private GameObject lf;

    private void Start()
    {
        GameObject.Find("MultiTargetCamera").GetComponent<MultiTargetCamera>().AddToView(transform.Find("Body/Stomach/Hip").transform);
    }
    
    public void Instantiate(string character)
    {
        rf = transform.Find("Body/RUA/RLA/RF").gameObject;
        lf = transform.Find("Body/LUA/LLA/LF").gameObject;
        switch (character)
        {
            case "Chronopen":
                characterScale = new Vector3(1.0f, 1.0f, 1.0f);
                usesWeapon = true;
                isTwoHanded = true;
                characterRadius = 10.0f;
                characterSpeed = 50.0f;
                characterHealth = 200.0f;
                characterCooldown = 10.0f;
                characterDamage = 10.0f;
                characterKnockback = 25.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Chronopen>();
                break;
            case "Holstar":
                characterScale = new Vector3(1.0f, 1.0f, 1.0f);
                usesWeapon = true;
                isTwoHanded = false;
                characterRadius = 10.0f;
                characterSpeed = 50.0f;
                characterHealth = 160.0f;
                characterCooldown = 6.0f;
                characterDamage = 12.5f;
                characterKnockback = 25.0f;
                characterSkillDuration = 0.0f;
                gameObject.AddComponent<Holstar>();
                break;
            case "Stele":
                characterScale = new Vector3(0.8f, 0.8f, 0.8f);
                usesWeapon = true;
                isTwoHanded = true;
                characterRadius = 8.0f;
                characterSpeed = 60.0f;
                characterHealth = 160.0f;
                characterCooldown = 15.0f;
                characterDamage = 12.5f;
                characterKnockback = 20.0f;
                characterSkillDuration = 0.0f;
                gameObject.AddComponent<Stele>();
                break;
            case "Pugilse":
                characterScale = new Vector3(0.8f, 0.8f, 0.8f);
                usesWeapon = true;
                isTwoHanded = true;
                characterRadius = 8.0f;
                characterSpeed = 60.0f;
                characterHealth = 160.0f;
                characterCooldown = 15.0f;
                characterDamage = 8.0f;
                characterKnockback = 20.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Pugilse>();
                break;
        }
        GetComponent<Skill>().SetStats(characterCooldown, characterSkillDuration);
        GetComponent<Health>().SetHealth(characterHealth);
        BounceOnImpact[] bounceOnImpacts = GetComponentsInChildren<BounceOnImpact>();
        foreach (BounceOnImpact bounceOnImpact in bounceOnImpacts)
        {
            bounceOnImpact.SetKnockback(characterKnockback);
        }
        transform.Find("Body").GetComponent<Movement>().SetSpeed(characterSpeed);
        Damage[] damages = GetComponentsInChildren<Damage>();
        foreach (Damage damage in damages)
        {
            damage.SetDamage(characterDamage);
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
    }

    public Vector3 GetAvailablePosition(GameObject caller, Vector3 position)
    {
        float callerRadius = caller.GetComponent<CharacterManager>().characterRadius;
        Vector3 callerHipPosition = caller.transform.Find("Body/Stomach/Hip").position;
        Vector3 callerEndPosition = position;
        float callerVectorLenght = (callerEndPosition - callerHipPosition).magnitude;
        Vector3 callerVector = (callerEndPosition - callerHipPosition).normalized;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Dictionary<GameObject, (float radius, Vector3 hipPosition)> playerData = new Dictionary<GameObject, (float, Vector3)>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == caller)
            {
                continue;
            }
            CharacterManager characterManager = players[i].GetComponent<CharacterManager>();
            Transform hip = players[i].transform.Find("Body/Stomach/Hip");

            if (characterManager != null && hip != null)
            {
                playerData[players[i]] = (characterManager.characterRadius, hip.position);
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = player.Value.radius + callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght += range * 1.5f;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght += callerRadius;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        if (callerEndPosition.x > roomPositiveDimensions.x)
        {
            callerEndPosition.x = roomPositiveDimensions.x - callerRadius / 2;
        }
        else if (callerEndPosition.x < roomNegativeDimensions.x)
        {
            callerEndPosition.x = roomNegativeDimensions.x + callerRadius / 2;
        }
        if (callerEndPosition.y > roomPositiveDimensions.y)
        {
            callerEndPosition.y = roomPositiveDimensions.y - callerRadius / 2;
        }
        else if (callerEndPosition.y < roomNegativeDimensions.y)
        {
            callerEndPosition.y = roomNegativeDimensions.y + callerRadius / 2;
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = player.Value.radius + callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght -= range * 1.5f;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        foreach(KeyValuePair<GameObject, (float radius, Vector3 hipPosition)> player in playerData)
        {
            float range = callerRadius;
            if (Vector3.Distance(callerEndPosition, player.Value.hipPosition) <= range)
            {
                callerVectorLenght -= callerRadius;
                callerEndPosition = callerHipPosition + callerVector * callerVectorLenght;
            }
        }
        return callerEndPosition;
    }
}
