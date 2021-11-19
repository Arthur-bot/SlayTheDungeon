using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : CardEffect
{
    [SerializeField] private int value;

    public override void ApplyEffect(List<Enemy> targets)
    {
        foreach (var target in targets)
        {
            target.TakeDamage(value);
        }
    }

    public override void ApplyEffect(CharacterData target)
    {
        target.TakeDamage(value);
    }
}
