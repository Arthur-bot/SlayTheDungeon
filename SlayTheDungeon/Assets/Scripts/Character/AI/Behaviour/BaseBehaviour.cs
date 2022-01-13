using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBehaviour
{
    protected List<KeyWord> preferedKeywords;
    protected Boss owner;

    public Boss Owner { get => owner; set => owner = value; }

    // LookFor is the base function to use when looking for a specific card or effect
    public virtual CardData LookForCard()
    {
        CardData perfectCard = null;
        int nbMatchingKeywords;
        int maxMatchingKeywords = 0;
        foreach (CardData card in owner.Hand)
        {
            nbMatchingKeywords = 0;
            foreach (KeyWord keyword in preferedKeywords)
            {
                if (card.Keywords.Contains(keyword))
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
