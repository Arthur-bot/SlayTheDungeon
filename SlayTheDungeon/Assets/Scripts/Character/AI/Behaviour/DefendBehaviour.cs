using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBehaviour : BaseBehaviour
{
    protected int defenseCount/*, defenseMean, passedTurns*/;

    public DefendBehaviour()
    {
        preferedKeywords = new List<KeyWord> { KeyWord.Defend};
    }

    public override CardData LookForCard()
    {
        CardData perfectCard = null;
        int nbMatchingKeywords;
        int maxMatchingKeywords = 0;

        foreach (CardData card in owner.Hand)
        {
            nbMatchingKeywords = 0;
            foreach (KeyWord keyword in preferedKeywords)
            {
                if ((keyword != KeyWord.Defend || owner.Stats.CurrentArmor < owner.DamageTaken) && card.Keywords.Contains(keyword))
                {
                    nbMatchingKeywords++;
                    if (nbMatchingKeywords > maxMatchingKeywords)
                    {
                        perfectCard = card;
                        maxMatchingKeywords = nbMatchingKeywords;
                    }
                }
            }
        }
        if (perfectCard != null)
            Debug.Log(perfectCard.CardName);
        return perfectCard;
    }
}
