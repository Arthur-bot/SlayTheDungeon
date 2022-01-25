using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : CardEffect
{
    [SerializeField] private bool drainLife;
    [SerializeField] private bool sacrifice;
    public override void ApplyEffect(CharacterData caster, List<Enemy> targets)
    {
        int effectValue = value;
        if (TargetType != Target.Self)
        {
            effectValue += caster.Stats.CurrentFury;
            caster.Stats.ChangeFury(-caster.Stats.CurrentFury);
        }
        foreach (var target in targets)
        {
            int remainedHealth = target.Stats.CurrentHealth;
            int damage = target.TakeDamage(effectValue);
            bool kill = damage >= remainedHealth;
            if ((drainLife || (kill & sacrifice)) && damage > 0)
            {
                caster.Stats.ChangeHealth(damage);
                GameUI.Instance.DamageUI.NewDamage(-damage, caster.transform.position);
            }
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
        int remainedHealth = target.Stats.CurrentHealth;
        int damage = target.TakeDamage(effectValue);
        bool kill = damage >= remainedHealth;
        if ((drainLife || (kill & sacrifice)) && damage > 0)
        {
            caster.Stats.ChangeHealth(damage);
            GameUI.Instance.DamageUI.NewDamage(-damage, caster.transform.position);
        }
    }

    public override int GetEffectValue()
    {
        return value;
    }

    public override Sprite GetIcon()
    {
        if (!drainLife) return DataBase.Instance.AttackIcon;
        else return DataBase.Instance.DrainIcon;
    }
}
