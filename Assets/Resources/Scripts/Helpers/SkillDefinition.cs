
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public enum EffectTrigger { Kill, SkillUse }

[Serializable]
public struct SkinEffect
{
    public EffectTrigger trigger;
    public GameObject prefab;
}

[CreateAssetMenu(menuName = "Game/Skin Definition")]
public class SkinDefinition : ScriptableObject
{
    [Header("Identity")]
    public string skinName;

    [Header("Applicability")]
    public CharacterSize sizeCategory;
    public bool isGlobal;
    public List<CharacterDefinition> specificCharacters;

    [Header("Visual Overrides")]
    public SkinPart[] parts;

    [Header("Modular Effects")]
    public SkinEffect[] effects;
}