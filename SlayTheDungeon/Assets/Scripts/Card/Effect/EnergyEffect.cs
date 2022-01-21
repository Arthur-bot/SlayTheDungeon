using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEffect : CardEffect
{
    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.GetEnergy(value);
    }
}
