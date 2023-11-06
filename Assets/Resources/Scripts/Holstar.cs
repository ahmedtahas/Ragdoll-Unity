using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holstar : MonoBehaviour
{
    private SkillStick skillStick;
    private Transform rf;
    private Vector2 barrelPosition;
    private GameObject bulletPrefab;
    private Rigidbody2D rfRigidbody;
    public float recoilForce = 100f;

    void Start()
    {
        skillStick = GameObject.Find("SkillStick").GetComponent<SkillStick>();
        if (skillStick != null)
        {
            skillStick.SetBehavior(SkillStick.BehaviorType.Click);
            skillStick.OnClick += HandleSignal;
        }
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        rf = transform.Find("Body/RUA/RLA/RF");
        rfRigidbody = rf.GetComponent<Rigidbody2D>();
    }

    void HandleSignal(bool isReleased)
    {
        Debug.Log("Received signal: " + isReleased);
        if (isReleased)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        
        barrelPosition = rf.GetComponent<WeaponCollision>().GetUpperRightCorner();
        
        GameObject bullet = Instantiate(bulletPrefab, barrelPosition, rf.rotation) as GameObject;
        // Get the Collider2D component of the bullet
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();

        // Get the Collider2D components of all the player's children
        Collider2D[] playerChildrenColliders = GetComponentsInChildren<Collider2D>();

        // Make the bullet ignore collision with all the player's children
        foreach (Collider2D childCollider in playerChildrenColliders)
        {
            Physics2D.IgnoreCollision(bulletCollider, childCollider);
        }
        
        rfRigidbody.AddForce(-rf.right * recoilForce, ForceMode2D.Impulse);
    }
}
