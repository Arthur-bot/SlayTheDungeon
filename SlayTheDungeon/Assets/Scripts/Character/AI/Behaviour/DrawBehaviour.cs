using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBehaviour : BaseBehaviour
{
    private bool onlyFreeDraw;
    public DrawBehaviour()
    {
        preferedKeywords = new List<KeyWord> { KeyWord.Draw };
    }
    public override List<CardData> LookForCards()
    {
        if (!onlyFreeDraw)
        {
            return base.LookForCards();
        }
        List<CardData> perfectCards = new List<CardData>();
        int nbMatchingKeywords;
        int maxMatchingKeywords = 0;
        foreach (CardData card in owner.Hand)
        {
            nbMatchingKeywords = 0;
            foreach (KeyWord keyword in preferedKeywords)
            {
                if (card.Keywords.Contains(keyword) && (card.Cost == 0 || card.Keywords.Contains(KeyWord.Energy)))
                {
                    nbMatchingKeywords++;
                    if (nbMatchingKeywords >= maxMatchingKeywords)
                    {
                        if (nbMatchingKeywords > maxMatchingKeywords) perfectCards.Clear();
                        perfectCards.Add(card);
                        maxMatchingKeywords = nbMatchingKeywords;
                    }
                }
            }
        }
        return perfectCards;
    }
    public bool FreeDraw { get => onlyFreeDraw; set => onlyFreeDraw = value; }
}
