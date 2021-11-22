using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pile : MonoBehaviour
{
    // Protected variables
    [SerializeField] protected CardUI cardTemplate;
    [SerializeField] protected List<CardData> cards;
    [SerializeField] protected TextMeshProUGUI countText;

    // Public Functions
    public void AddCard(CardData cardData)
    {
        cards.Add(cardData);
        countText.text = cards.Count.ToString();
    }
    public void ShowCards()
    {
        foreach (CardData card in cards)
        {
            // nothing to do
        }
    }
}
