using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEffect : CardEffect
{
    [SerializeField] private int value;

    public override void ApplyEffect(CharacterData target)
    {
        target.Stats.ChangeArmor(value);
    }
}
