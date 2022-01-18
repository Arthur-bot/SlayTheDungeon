using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrap : Trap
{
    public override void TriggerTrap(PlayerData target)
    {
        alreadyTriggered = true;
        target.RemoveCard(target.Deck[Random.Range(0, target.Deck.Count)]);
    }

}
