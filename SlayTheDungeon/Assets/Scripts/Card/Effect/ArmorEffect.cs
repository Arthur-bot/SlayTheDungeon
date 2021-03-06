using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEffect : CardEffect
{
    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.Stats.ChangeArmor(value);
    }

    public override int GetEffectValue()
    {
        return value;
    }

    public override Sprite GetIcon()
    {
        return DataBase.Instance.ArmorIcon;
    }
}
