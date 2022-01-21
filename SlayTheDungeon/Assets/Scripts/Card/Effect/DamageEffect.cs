using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : CardEffect
{
    [SerializeField] private bool drainLife;
    public override void ApplyEffect(CharacterData caster, List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            int damage = target.TakeDamage(value + caster.Stats.CurrentFury);
            if (drainLife) caster.Stats.ChangeHealth(damage);
        }
        caster.Stats.ChangeFury(-caster.Stats.CurrentFury);
    }

    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        int damage = target.TakeDamage(value + caster.Stats.CurrentFury);
        if (drainLife) caster.Stats.ChangeHealth(damage);
        caster.Stats.ChangeFury(-caster.Stats.CurrentFury);
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
