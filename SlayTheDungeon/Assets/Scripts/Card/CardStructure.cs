using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

[System.Serializable]
public class EffectStructure
{
    public string name;
    public string targetType;
    public int value;

}


