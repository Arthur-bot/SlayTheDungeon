using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : CardEffect
{
    [SerializeField] private int damage;
    [SerializeField] private int numberOfTurn;
    [SerializeField] private Sprite effectSprite;

    public override void ApplyEffect(List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            ElementalEffect effect = new ElementalEffect(numberOfTurn, StatSystem.DamageType.POISON, damage, effectSprite);

            target.Stats.AddElementalEffect(effect);
        }
    }

    public override void ApplyEffect(CharacterData target)
    {
        ElementalEffect effect = new ElementalEffect(numberOfTurn, StatSystem.DamageType.POISON, damage, effectSprite);

        target.Stats.AddElementalEffect(effect);
    }
}
