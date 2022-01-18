using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Trap
{
    [SerializeField] private int damage;
    public override void TriggerTrap(PlayerData target)
    {
        alreadyTriggered = true;
        target.TakeDamage(damage);
    }
}
