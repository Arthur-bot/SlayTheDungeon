using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected List<KeyWord> preferedKeywords;
    private Boss owner;
        
    // Start is called before the first frame update, during this, we define prefered keywords and the data we wish to use to make decisions
    void Start()
    {
        owner = this.GetComponent<Boss>();
    }

    protected void TakeDecision()
    {

    }

    // LookFor is the base function to use when looking for a specific card or effect
    private CardData LookForCard()
    {
        CardData perfectCard = null;
        int nbMatchingKeywords;
        int maxMatchingKeywords = 0;

        foreach (CardData card in owner.hand)
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
        return perfectCard;
    }
}
