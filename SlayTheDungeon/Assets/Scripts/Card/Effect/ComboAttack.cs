using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : CardEffect
{
    public override void ApplyEffect(CharacterData caster,List<Enemy> targets)
    {
        int effectValue = value;
        if (TargetType != Target.Self)
        {
            effectValue += caster.Stats.CurrentFury;
            caster.Stats.ChangeFury(-caster.Stats.CurrentFury);
        }
        foreach (var target in targets)
        {
            target.TakeDamage(effectValue + BattleData.Instance.NbPlayedCard);
        }
    }

    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        int effectValue = value;
        if (TargetType != Target.Self)
        {
            effectValue += caster.Stats.CurrentFury;
            caster.Stats.ChangeFury(-caster.Stats.CurrentFury);
        }
        target.TakeDamage(effectValue + BattleData.Instance.NbPlayedCard);
    }

    public override int GetEffectValue()
    {
        return value;
    }
}
