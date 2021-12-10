using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : CardEffect
{
    [SerializeField] private int numberOfStack;
    [SerializeField] private Sprite effectSprite;

    public override void ApplyEffect(List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            PoisonEffect effect = new PoisonEffect(numberOfStack, effectSprite);

            target.Stats.AddElementalEffect(effect);
        }
    }

    public override void ApplyEffect(CharacterData target)
    {
        PoisonEffect effect = new PoisonEffect(numberOfStack, effectSprite);

        target.Stats.AddElementalEffect(effect);
    }
}
