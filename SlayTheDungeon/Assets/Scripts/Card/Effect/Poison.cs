using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : CardEffect
{

    public override void ApplyEffect(List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            PoisonEffect effect = new PoisonEffect(value, DataBase.Instance.PoisonIcon);

            target.Stats.AddElementalEffect(effect);
        }
    }

    public override void ApplyEffect(CharacterData target)
    {
        PoisonEffect effect = new PoisonEffect(value, DataBase.Instance.PoisonIcon);

        target.Stats.AddElementalEffect(effect);
    }

    public override int GetEffectValue()
    {
        return value;
    }

    public override Sprite GetIcon()
    {
        return DataBase.Instance.PoisonIcon;
    }
}
