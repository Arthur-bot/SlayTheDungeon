using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    // Protected variables
    [SerializeField] protected CardUI cardTemplate;
    [SerializeField] protected List<CardData> cards;

    // Public Functions
    public void AddCard(CardData cardData)
    {
        cards.Add(cardData);
    }
    public void ShowCards()
    {
        foreach (CardData card in cards)
        {
            // nothing to do
        }
    }
}
