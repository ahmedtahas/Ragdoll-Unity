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
    public const string GAME_MODE = "GameMode";
    public const string SINGLE_PLAYER = "SinglePlayer";
    public const string MULTI_PLAYER = "MultiPlayer";
    public static Vector3 LEFT_STICK = new Vector3(480.0f, 0.0f, 0.0f);
    public static Vector3 RIGHT_STICK = new Vector3(-480.0f, 0.0f, 0.0f);
    public static Vector3 LEFT_KNOB = new Vector3(224.0f, -284.0f, 0.0f);
    public static Vector3 RIGHT_KNOB = new Vector3(-224.0f, -284.0f, 0.0f);
    public const string BOT = "Bot";
    public const string CHRONOPEN = "Chronopen";
    public const string TIN = "Tin";
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
        { ROOT, "Comet of Hate" },
        { TIN, "Singularity"}
    };
    public static readonly Dictionary<string, string> CHARACTER_SKILL_DESCRIPTIONS = new Dictionary<string, string>
    {
        { CHRONOPEN, "Save your position in time and return to that moment after a duration. Any damage taken before returning will be healed back, with an extra portion on top.\nCooldown: 10s\nDuration: 10s\nUsage: Tap" },
        { HOLSTAR, "Fire your gun.\nCooldown: 6s\nUsage: Tap" },
        { STELE, "Throw a dagger towards your target and instantly teleport to its location. The dagger will damage opponents, but you need to land two hits on an enemy before you can throw another one.\nCooldown: 15s\nUsage: Aim" },
        { PUGILSE, "Combo on every hit that deals damage and then gain damage according to the combo count for a duration. The duration extends with combo count.\nCooldown: 15s\nMax Combo: 10\nUsage: Tap" },
        { ROOT, "Summon a Comet that is locked on to the enemy for the duration. The Comet can also damage you if hit.\nCooldown: 15s\nDuration: 10s\nUsage: Tap" },
        { TIN, "Create a singularity that pulls enemies. The singularity can also damage enemies.\nCooldown: 15s\nDuration: 4s\nUsage: Hold" }
    };
    public static readonly Dictionary<string, int> CHARACTER_HEALTH_POINTS = new Dictionary<string, int>
    {
        { CHRONOPEN, 200 },
        { HOLSTAR, 160 },
        { STELE, 160 },
        { PUGILSE, 160 },
        { ROOT, 160 },
        { TIN, 240 }
    };
    public static readonly Dictionary<string, int> CHARACTER_SPEEDS = new Dictionary<string, int>
    {
        { CHRONOPEN, 50 },
        { HOLSTAR, 50 },
        { STELE, 60 },
        { PUGILSE, 60 },
        { ROOT, 50 },
        { TIN, 40 }
    };
    public static readonly Dictionary<string, float> CHARACTER_DAMAGES = new Dictionary<string, float>
    {
        { CHRONOPEN, 10.0f },
        { HOLSTAR, 12.5f },
        { STELE, 12.5f },
        { PUGILSE, 8.0f },
        { ROOT, 12.5f },
        { TIN, 8.0f }
    };

    public static readonly Dictionary<int, string> CHARACTER_NAMES = new Dictionary<int, string>
    {
        { 1, Constants.CHRONOPEN },
        { 2, Constants.HOLSTAR },
        { 3, Constants.STELE },
        { 4, Constants.PUGILSE },
        { 5, Constants.ROOT },
        { 6, Constants.TIN }
    };

}
