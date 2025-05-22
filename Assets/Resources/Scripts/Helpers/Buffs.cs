using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{

    public void SetCooldownAndDuration(float characterCooldown, float characterSkillDuration)
    {
        GetComponent<Skill>().SetStats(characterCooldown, characterSkillDuration);
    }
    public void SetHealth(float characterHealth)
    {
        GetComponent<Health>().SetHealth(characterHealth);
    }
    public void SetKnockback(float characterKnockback)
    {
        BounceOnImpact[] bounceOnImpacts = GetComponentsInChildren<BounceOnImpact>();
        foreach (BounceOnImpact bounceOnImpact in bounceOnImpacts)
        {
            bounceOnImpact.SetKnockback(characterKnockback);
        }
    }
    public void SetDamage(float characterDamage)
    {
        Damage[] damages = GetComponentsInChildren<Damage>();
        foreach (Damage damage in damages)
        {
            damage.SetDamage(characterDamage);
        }
    }

    public void BuffByLevel(int level)
    {
        GetComponent<Health>().SetHealth(GetComponent<Health>().GetMaxHealth() * (1 + level * 0.1f));
        Damage[] damages = GetComponentsInChildren<Damage>();
        foreach (Damage damage in damages)
        {
            damage.SetDamage(damage.GetDamage() * (1 + level * 0.1f));
        }
    }
}
