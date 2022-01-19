using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinCostBehaviour : BaseBehaviour
{
    private int limitCost = 1000;

    public int LimitCost { get => limitCost; set => limitCost = value; }

    public override List<CardData> LookForCards()
    {
        int minCost = limitCost;
        List<CardData> perfectCards = new List<CardData>();
        foreach (CardData card in owner.Hand)
        {
            if (card.Cost <= minCost)
            {
                if (card.Cost < minCost)
                {
                    perfectCards.Clear();
                }
                minCost = card.Cost;
                perfectCards.Add(card);
            }
        }
        return perfectCards;
    }
}
