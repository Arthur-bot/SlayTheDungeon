using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pile : MonoBehaviour
{
    // Protected variables
    [SerializeField] protected CardUI cardTemplate;
    [SerializeField] protected TextMeshProUGUI countText;
    [SerializeField] private ScrollCardView scrollCards;
    [SerializeField] private GameObject scrollPanel;

    [SerializeField] protected List<CardData> cards = new List<CardData>();

    public List<CardData> Cards { get => cards; set => cards = value; }

    // Public Functions
    public void AddCard(CardData cardData)
    {
        cards.Add(cardData);
        countText.text = cards.Count.ToString();
    }

    public void ShowCards()
    {
        scrollPanel.SetActive(true);
        scrollCards.ShowCards(cards);
    }
}
