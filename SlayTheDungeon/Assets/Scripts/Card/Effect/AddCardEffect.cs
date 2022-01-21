using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardEffect : CardEffect
{
    [SerializeField] private List<CardData> toAdd;

    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.AddCards(toAdd);
    }
}
