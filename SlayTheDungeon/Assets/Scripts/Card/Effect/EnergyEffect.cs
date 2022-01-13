using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEffect : CardEffect
{
    public override void ApplyEffect(CharacterData target)
    {
        if (target is PlayerData)
            (target as PlayerData).ChangeEnergy(value);
    }
}
