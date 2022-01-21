using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFury : CardEffect
{
    public override void ApplyEffect(CharacterData caster, CharacterData target)
    {
        target.Stats.ChangeFury(target.Stats.CurrentFury);
    }
}
