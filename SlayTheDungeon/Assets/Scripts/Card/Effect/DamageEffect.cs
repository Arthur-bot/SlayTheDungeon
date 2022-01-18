using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : CardEffect
{
    public override void ApplyEffect(List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(value);
        }
    }

    public override void ApplyEffect(CharacterData target)
    {
        target.TakeDamage(value);
    }

    public override int GetEffectValue()
    {
        return value;
    }

    public override Sprite GetIcon()
    {
        return DataBase.Instance.AttackIcon;
    }
}
