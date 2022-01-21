using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryEffect : CardEffect
{
    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.Stats.ChangeFury(value);
        Debug.Log(target.Stats.CurrentFury);
    }

    public override int GetEffectValue()
    {
        return value;
    }
}
