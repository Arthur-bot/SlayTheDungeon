using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : Pile
{
    private HandAnimation handAnimation;

    private void Start()
    {
        handAnimation = HandAnimation.Instance;
        countText.text = cards.Count.ToString();
    }
    public void Shuffle(Pile deck)
    {
        int cardCounter = 0;
        foreach (CardData card in cards)
        {
            if (cardCounter < 5)
            {
                // Creates a new empty card
                var newCard = Instantiate(cardTemplate, transform);
                // Initializes it whit the random card
                newCard.SetupCard(card);
                handAnimation.ShuffleAnimation(newCard.GetComponent<RectTransform>());
                // Removes the card from the pile
                cardCounter++;
            }
            deck.AddCard(card);
        }
        cards.Clear();
    }
}
