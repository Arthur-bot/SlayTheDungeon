 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEffect : CardEffect
{
    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.DrawCards(value);
    }
}
