using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : CardEffect
{
    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        DodgeEffect effect = new DodgeEffect(value, DataBase.Instance.DodgeIcon);

        target.Stats.AddElementalEffect(effect);
    }

    public override int GetEffectValue()
    {
        return value;
    }

    public override Sprite GetIcon()
    {
        return DataBase.Instance.DodgeIcon;
    }
}
