using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEffect : CardEffect
{
    [SerializeField] private int value;
    public override void ApplyEffect(CharacterData target)
    {
        if (target is PlayerData)
            (target as PlayerData).ChangeEnergy(value);
    }
}
