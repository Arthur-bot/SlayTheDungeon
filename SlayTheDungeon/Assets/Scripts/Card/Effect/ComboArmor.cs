using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboArmor : CardEffect
{
    public override void ApplyEffect(CharacterData target)
    {
        target.Stats.ChangeArmor(value + BattleData.Instance.NbPlayedCard);
    }
}
