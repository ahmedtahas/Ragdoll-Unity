using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const string CHARACTER_PREFAB_PATH = "Prefabs/Character";
    public const string METEOR_PREFAB_PATH = "Prefabs/Meteor";
    public const string CHARACTER_DISPLAY_LAYER = "CharacterDisplay";
    public const string DAGGER_PREFAB_PATH = "Prefabs/Dagger";
    public const string BULLET_PREFAB_PATH = "Prefabs/Bullet";
    public const string CHRONOPEN = "Chronopen";
    public const string HOLSTAR = "Holstar";
    public const string ROOT = "Root";
    public const string STELE = "Stele";
    public const string PUGILSE = "Pugilse";
    public const string SELECTED_CHARACTER = "SelectedCharacter";
    public const string HIP = "Body/Stomach/Hip";
    public const string RF = "Body/RUA/RLA/RF";
    public const string LF = "Body/LUA/LLA/LF";
    public const string BODY = "Body";
    public const string MTC = "MultiTargetCamera";
    public static readonly List<string> CHARACTERS = new List<string> { CHRONOPEN, HOLSTAR, STELE, PUGILSE, ROOT };
    public static readonly Dictionary<string, string> CHARACTER_SKILL_NAMES = new Dictionary<string, string>
    {
        { CHRONOPEN, "Time in a Bottle" },
        { HOLSTAR, "Bullseye" },
        { STELE, "Nothing Personal" },
        { PUGILSE, "Rhythm of Stars" },
        { ROOT, "Comet of Hate" }
    };
    public static readonly Dictionary<string, string> CHARACTER_SKILL_DESCRIPTIONS = new Dictionary<string, string>
    {
        { CHRONOPEN, "Chronopen saves its position in time and then returns to that moment after the duration. Damage taken before returning will be healed back with an extra portion of it.\nCooldown: 10s\nDuration: 10s" },
        { HOLSTAR, "Holstar shoots a bullet in the direction of the gun.\nCooldown: 6s" },
        { STELE, "Stele throws a dagger in a desired position and then teleports to it. The dagger can hit enemies and deal damage. Stele needs to damage the enemy 2 times to be able to throw a dagger.\nCooldown: 15s" },
        { PUGILSE, "Pugilse combos on every hit that deals damage and then gains damage according to the combo count for a duration. The duration extends with combo count.\nCooldown: 15s\nMax Combo: 10\n" },
        { ROOT, "Root summons a Comet and guides it to the enemy for a duration. The Comet can also damage Root if hit.\nCooldown: 15s\nDuration: 10s" }
    };
    public static readonly Dictionary<string, int> CHARACTER_HEALTH_POINTS = new Dictionary<string, int>
    {
        { CHRONOPEN, 200 },
        { HOLSTAR, 160 },
        { STELE, 160 },
        { PUGILSE, 160 },
        { ROOT, 160 }
    };
    public static readonly Dictionary<string, int> CHARACTER_SPEEDS = new Dictionary<string, int>
    {
        { CHRONOPEN, 50 },
        { HOLSTAR, 50 },
        { STELE, 60 },
        { PUGILSE, 60 },
        { ROOT, 50 }
    };
    public static readonly Dictionary<string, float> CHARACTER_DAMAGES = new Dictionary<string, float>
    {
        { CHRONOPEN, 10.0f },
        { HOLSTAR, 12.5f },
        { STELE, 12.5f },
        { PUGILSE, 8.0f },
        { ROOT, 12.5f }
    };

}
