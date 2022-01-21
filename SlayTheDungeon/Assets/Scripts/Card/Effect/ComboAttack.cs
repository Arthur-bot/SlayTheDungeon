using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : CardEffect
{
    public override void ApplyEffect(CharacterData caster,List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(value + BattleData.Instance.NbPlayedCard);
        }
    }

    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.TakeDamage(BattleData.Instance.NbPlayedCard);
    }

    public override int GetEffectValue()
    {
        return value;
    }
}
