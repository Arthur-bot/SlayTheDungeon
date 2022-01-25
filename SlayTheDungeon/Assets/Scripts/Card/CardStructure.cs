using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardStructure
{
    public string spritePath;
    public int cost;
    public string cardName;
    public string description;
    public AudioClip cardSoundEffect;
    public List<EffectStructure> cardEffects;
    public bool limitedUse;
    public int nbUse;
}

[Serializable]
public class MonsterStructure
{
    public string spritePath;
    public string name;
    public int difficulty;
    public StatsStructure stats;
    public List<EffectStructure> attacks;
}

[Serializable]
public class EffectStructure
{
    public string name;
    public string targetType;
    public int value;
}

[Serializable]
public class StatsStructure
{
    public int health;
    public int maxArmor;
    public int maxFury;
}

[Serializable]
public class ConditionalEffectStructure
{
    public string name;
    public string targetType;
    public int value;
    public string conditionalStats;
    public string condition;
    public EffectStructure effectPlayed;
}
