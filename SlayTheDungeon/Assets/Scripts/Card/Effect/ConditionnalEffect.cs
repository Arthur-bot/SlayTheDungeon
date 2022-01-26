using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConditionnalEffect : CardEffect
{
    public enum ConditionalStat
    {
        Health,
        Armor,
        CardsPlayed,
        CardsInHand
    }

    public enum Condition
    {
        Greater,
        Lower
    }

    public ConditionalStat conditionalStat;
    public Condition condition;
    public CardEffect EffectPlayed;

    #region Public Methods

    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        var condition = this.condition switch
        {
            Condition.Greater => conditionalStat switch
            {
                ConditionalStat.Health => caster.Stats.CurrentHealth > value,
                ConditionalStat.Armor => caster.Stats.CurrentArmor > value,
                ConditionalStat.CardsInHand => GameUI.Instance.PlayerHand.GetNumberOfCard() > value,
                ConditionalStat.CardsPlayed => BattleData.Instance.NbPlayedCard > value,
                _ => throw new ArgumentOutOfRangeException()
            },
            Condition.Lower => conditionalStat switch
            {
                ConditionalStat.Health => caster.Stats.CurrentHealth < value,
                ConditionalStat.Armor => caster.Stats.CurrentArmor < value,
                ConditionalStat.CardsInHand => GameUI.Instance.PlayerHand.GetNumberOfCard() < value,
                ConditionalStat.CardsPlayed => BattleData.Instance.NbPlayedCard < value,
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => false
        };

        if (condition)
        {
            EffectPlayed.ApplyEffect(caster, target);
        }
    }

    public override void ApplyEffect(CharacterData caster, List<Enemy> targets)
    {
        var condition = this.condition switch
        {
            Condition.Greater => conditionalStat switch
            {
                ConditionalStat.Health => caster.Stats.CurrentHealth > value,
                ConditionalStat.Armor => caster.Stats.CurrentArmor > value,
                ConditionalStat.CardsInHand => GameUI.Instance.PlayerHand.GetNumberOfCard() > value,
                ConditionalStat.CardsPlayed => BattleData.Instance.NbPlayedCard > value,
                _ => throw new ArgumentOutOfRangeException()
            },
            Condition.Lower => conditionalStat switch
            {
                ConditionalStat.Health => caster.Stats.CurrentHealth < value,
                ConditionalStat.Armor => caster.Stats.CurrentArmor < value,
                ConditionalStat.CardsInHand => GameUI.Instance.PlayerHand.GetNumberOfCard() < value,
                ConditionalStat.CardsPlayed => BattleData.Instance.NbPlayedCard < value,
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => false
        };

        if (condition)
        {
            EffectPlayed.ApplyEffect(caster, targets);
        }
    }

    public override int GetEffectValue()
    {
        return value;
    }

    public void EffectFromJson(ConditionalEffectStructure effectStructure)
    {
        targetType = (Target)Enum.Parse(typeof(Target), effectStructure.targetType);
        value = effectStructure.value;
        conditionalStat = (ConditionalStat)Enum.Parse(typeof(ConditionalStat), effectStructure.conditionalStats);
        condition = (Condition)Enum.Parse(typeof(Condition), effectStructure.condition);

        var effect = CreateInstance(effectStructure.effectPlayed.name) as CardEffect;

        if (effect == null) return;

        effect.EffectFromJson(effectStructure.effectPlayed);
        EffectPlayed = effect;

        //AssetDatabase.AddObjectToAsset(effect, this);
    }

    #endregion
}
