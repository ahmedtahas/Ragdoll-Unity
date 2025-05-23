using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using System;


public class CharacterManager : NetworkBehaviour
{
    public bool usesWeapon = false;
    public bool isTwoHanded = false;
    public Vector3 characterScale = new Vector3(1.0f, 1.0f, 1.0f);
    public float characterRadius = 50.0f;
    public float characterSpeed = 1.0f;
    public float characterHealth = 100.0f;
    public float characterCooldown = 10.0f;
    public float characterDamage = 10.0f;
    public float characterKnockback = 5.0f;
    public float characterSkillDuration = 10.0f;
    public GameObject rf;
    public GameObject lf;
    public Rigidbody2D[] rigidbodies;

    Vector3 bigSize = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 mediumSize = new Vector3(1.0f, 1.0f, 1.0f);
    Vector3 smallSize = new Vector3(0.8f, 0.8f, 0.8f);
    float bigRadius = 12.5f;
    float mediumRadius = 10.0f;
    float smallRadius = 8.0f;
    float highSpeed = 120.0f;
    float mediumSpeed = 100.0f;
    float lowSpeed = 80.0f;
    float highHealth = 240.0f;
    float mediumHealth = 200.0f;
    float lowHealth = 160.0f;
    float highDamage = 12.5f;
    float mediumDamage = 10.0f;
    float lowDamage = 8.0f;
    float highKnockback = 25.0f;
    float mediumKnockback = 20.0f;
    float lowKnockback = 15.0f;


    public NetworkVariable<int> selection = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        selection.Value = 0;
        selection.OnValueChanged += (previousValue, newValue) =>
        {
            if (Constants.CHARACTER_NAMES.TryGetValue(newValue, out string value))
            {
                Instantiate(value);
            }
        };

        if (IsOwner)
        {
            selection.Value = PlayerPrefs.GetString(Constants.SELECTED_CHARACTER) switch
            {
                Constants.CHRONOPEN => 1,
                Constants.HOLSTAR => 2,
                Constants.STELE => 3,
                Constants.PUGILSE => 4,
                Constants.ROOT => 5,
                Constants.TIN => 6,
                Constants.DYNABULL => 7,
                Constants.DUPLICA => 8,
                Constants.CYROBYTE => 9,
                Constants.OBSCURON => 10,
                Constants.INFRADE => 11,
                Constants.ROARAK => 12,
                _ => 0
            };
        }
        ;
    }



    void OnEnable()
    {
        if (transform.parent != null)
        {
            transform.Find("UI").gameObject.SetActive(false);
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }


    void OnClientConnected(ulong clientId)
    {
        if (GameManager.Instance.playerTransform != null)
        {
            CharacterManager characterManager = GameManager.Instance.playerTransform.GetComponent<CharacterManager>();
            characterManager.selection.Value = PlayerPrefs.GetString(Constants.SELECTED_CHARACTER) switch
            {
                Constants.CHRONOPEN => 1,
                Constants.HOLSTAR => 2,
                Constants.STELE => 3,
                Constants.PUGILSE => 4,
                Constants.ROOT => 5,
                Constants.TIN => 6,
                Constants.DYNABULL => 7,
                Constants.DUPLICA => 8,
                Constants.CYROBYTE => 9,
                Constants.OBSCURON => 10,
                Constants.INFRADE => 11,
                Constants.ROARAK => 12,
                _ => 0
            };
        }
    }

    public void Instantiate(string characterName)
    {
        GameObject.Find(Constants.MTC).GetComponent<MultiTargetCamera>().AddToView(transform.Find(Constants.HIP).transform);
        string character = characterName;
        if (IsHost)
        {
            transform.Find(Constants.BODY).transform.position = new Vector3(-80, 0, 0);
        }
        else if (NetworkManager.Singleton.IsConnectedClient)
        {
            transform.Find(Constants.BODY).transform.position = new Vector3(+80, 0, 0);
        }
        gameObject.name = character;
        rf = transform.Find(Constants.RF).gameObject;
        lf = transform.Find(Constants.LF).gameObject;
        switch (character)
        {
            case Constants.BOT:
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = mediumHealth;
                characterDamage = mediumDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = false;
                isTwoHanded = false;
                characterCooldown = 0.0f;
                characterSkillDuration = 0.0f;
                gameObject.AddComponent<Bot>();
                GameManager.Instance.enemy = transform.Find(Constants.HIP).gameObject;
                GameManager.Instance.enemyTransform = transform;
                GameManager.Instance.enemyDamage = characterDamage;
                GameManager.Instance.enemyHealth = characterHealth;
                GameManager.Instance.enemyKnockback = characterKnockback;
                GameManager.Instance.enemyHealthComponent = GetComponent<Health>();
                break;
            case Constants.CHRONOPEN:
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = mediumHealth;
                characterDamage = mediumDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = true;
                isTwoHanded = true;
                characterCooldown = 10.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Chronopen>();
                break;
            case Constants.HOLSTAR:
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = lowHealth;
                characterDamage = highDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = true;
                isTwoHanded = false;
                characterCooldown = 6.0f;
                characterSkillDuration = 0.0f;
                gameObject.AddComponent<Holstar>();
                break;
            case Constants.STELE:
                characterScale = smallSize;
                characterRadius = smallRadius;
                characterSpeed = highSpeed;
                characterHealth = lowHealth;
                characterDamage = highDamage;
                characterKnockback = lowKnockback;
                usesWeapon = true;
                isTwoHanded = true;
                characterCooldown = 15.0f;
                characterSkillDuration = 0.0f;
                gameObject.AddComponent<Stele>();
                break;
            case Constants.PUGILSE:
                characterScale = smallSize;
                characterRadius = smallRadius;
                characterSpeed = highSpeed;
                characterHealth = lowHealth;
                characterDamage = lowDamage;
                characterKnockback = lowKnockback;
                usesWeapon = true;
                isTwoHanded = true;
                characterCooldown = 15.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Pugilse>();
                break;
            case Constants.ROOT:
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = lowHealth;
                characterDamage = highDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = true;
                isTwoHanded = false;
                characterCooldown = 15.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Root>();
                break;
            case Constants.TIN:
                characterScale = bigSize;
                characterRadius = bigRadius;
                characterSpeed = lowSpeed;
                characterHealth = highHealth;
                characterDamage = lowDamage;
                characterKnockback = highKnockback;
                usesWeapon = true;
                isTwoHanded = false;
                characterCooldown = 15.0f;
                characterSkillDuration = 4.0f;
                gameObject.AddComponent<Tin>();
                break;
            case Constants.DYNABULL:
                characterScale = bigSize;
                characterRadius = bigRadius;
                characterSpeed = lowSpeed;
                characterHealth = highHealth;
                characterDamage = lowDamage;
                characterKnockback = highKnockback;
                usesWeapon = true;
                isTwoHanded = true;
                characterCooldown = 15.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Dynabull>();
                break;
            case Constants.DUPLICA:
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = mediumHealth;
                characterDamage = mediumDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = false;
                isTwoHanded = false;
                characterCooldown = 20.0f;
                characterSkillDuration = 15.0f;
                gameObject.AddComponent<Duplica>();
                break;
            case Constants.KATE:
                character = Constants.DUPLICA;
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = mediumHealth;
                characterDamage = mediumDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = false;
                isTwoHanded = false;
                characterCooldown = 0.0f;
                characterSkillDuration = 0.0f;
                GetComponent<Movement>().SetSpeed(characterSpeed);
                GetComponent<Health>().enabled = false;
                GetComponent<Skill>().enabled = false;
                // GetComponent<Freezer>().enabled = false;
                GetComponent<Pusher>().enabled = false;
                break;
            case Constants.CYROBYTE:
                characterScale = smallSize;
                characterRadius = smallRadius;
                characterSpeed = highSpeed;
                characterHealth = lowHealth;
                characterDamage = highDamage;
                characterKnockback = lowKnockback;
                usesWeapon = true;
                isTwoHanded = true;
                characterCooldown = 15.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Cyrobyte>();
                break;
            case Constants.OBSCURON:
                characterScale = smallSize;
                characterRadius = smallRadius;
                characterSpeed = highSpeed;
                characterHealth = lowHealth;
                characterDamage = highDamage;
                characterKnockback = lowKnockback;
                usesWeapon = true;
                isTwoHanded = true;
                characterCooldown = 15.0f;
                characterSkillDuration = 10.0f;
                gameObject.AddComponent<Obscuron>();
                break;
            case Constants.INFRADE:
                characterScale = mediumSize;
                characterRadius = mediumRadius;
                characterSpeed = mediumSpeed;
                characterHealth = mediumHealth;
                characterDamage = mediumDamage;
                characterKnockback = mediumKnockback;
                usesWeapon = true;
                isTwoHanded = false;
                characterCooldown = 3.0f;
                characterSkillDuration = 15.0f;
                gameObject.AddComponent<Infrade>();
                break;
            case Constants.ROARAK:
                characterScale = bigSize;
                characterRadius = bigRadius;
                characterSpeed = lowSpeed;
                characterHealth = highHealth;
                characterDamage = lowDamage;
                characterKnockback = highKnockback;
                usesWeapon = true;
                isTwoHanded = false;
                characterCooldown = 25.0f;
                characterSkillDuration = 25.0f;
                gameObject.AddComponent<Roarak>();
                break;
        }
        rigidbodies = GetComponentsInChildren<Rigidbody2D>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Skins");
        foreach (Rigidbody2D rb in rigidbodies)
        {
            Sprite sprite = null;
            if (Constants.ARM_SKINS.Contains(rb.name))
            {
                sprite = sprites.FirstOrDefault(s => s.name.Equals(character + "_" + PlayerPrefs.GetInt(character + "ArmSkin", 1) + "_Arm", StringComparison.OrdinalIgnoreCase));
            }
            else if (Constants.LEG_SKINS.Contains(rb.name))
            {
                sprite = sprites.FirstOrDefault(s => s.name.Equals(character + "_" + PlayerPrefs.GetInt(character + "LegSkin", 1) + "_Leg", StringComparison.OrdinalIgnoreCase));
            }
            else if (Constants.HIP_SKINS.Contains(rb.name))
            {
                sprite = sprites.FirstOrDefault(s => s.name.Equals(character + "_" + PlayerPrefs.GetInt(character + "HipSkin", 1) + "_Hip", StringComparison.OrdinalIgnoreCase));
            }
            else if (rb.name == Constants.BODY_SKIN)
            {
                sprite = sprites.FirstOrDefault(s => s.name.Equals(character + "_" + PlayerPrefs.GetInt(character + "BodySkin", 1) + "_Body", StringComparison.OrdinalIgnoreCase));
            }
            else if (rb.name == Constants.HEAD_SKIN)
            {
                sprite = sprites.FirstOrDefault(s => s.name.Equals(character + "_" + PlayerPrefs.GetInt(character + "HeadSkin", 1) + "_Head", StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                sprite = sprites.FirstOrDefault(s => s.name.Equals(character + "_" + PlayerPrefs.GetInt(character + "WeaponSkin", 1) + "_" + rb.name, StringComparison.OrdinalIgnoreCase));
            }
            if (sprite != null)
            {
                rb.GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
        if (transform.parent != null)
        {
            return;
        }
        GetComponent<Skill>().SetStats(characterCooldown, characterSkillDuration);
        GetComponent<Health>().SetHealth(characterHealth);
        BounceOnImpact[] bounceOnImpacts = GetComponentsInChildren<BounceOnImpact>();
        foreach (BounceOnImpact bounceOnImpact in bounceOnImpacts)
        {
            bounceOnImpact.SetKnockback(characterKnockback);
        }
        GetComponent<Movement>().SetSpeed(characterSpeed);
        Damage[] damages = GetComponentsInChildren<Damage>();
        foreach (Damage damage in damages)
        {
            damage.SetDamage(characterDamage);
        }
        transform.localScale = characterScale;
        if (usesWeapon)
        {
            rf.GetComponent<WeaponCollision>().UpdateCollisionShape();
        }
        if (isTwoHanded)
        {
            lf.GetComponent<WeaponCollision>().UpdateCollisionShape();
        }
        if (character == Constants.BOT || characterName == Constants.KATE)
        {
            transform.Find("UI").gameObject.SetActive(false);
            return;
        }
        GameManager.Instance.player = transform.Find(Constants.HIP).gameObject;
        GameManager.Instance.playerTransform = transform;
        GameManager.Instance.playerDamage = characterDamage;
        GameManager.Instance.playerHealth = characterHealth;
        GameManager.Instance.playerKnockback = characterKnockback;
        GameManager.Instance.playerHealthComponent = GetComponent<Health>();
    }
}
