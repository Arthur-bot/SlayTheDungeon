using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    #region Fields

    [SerializeField] private Pile discardPile;
    private HandAnimation handAnimation;
    private RectTransform thisTransform;
    private List<CardUI> cards;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        handAnimation = GetComponent<HandAnimation>();
        cards = new List<CardUI>();
        thisTransform = GetComponent<RectTransform>();
        handAnimation.SetCards(cards);
    }

    #endregion

    #region Public Methods

    public void DiscardHand()
    {
        // Discards all cards
        foreach (CardUI card in cards)
        {
            handAnimation.DiscardAnimation(card.ThisTransform);
            discardPile.AddCard(card.Data);
        }
        // The hand is empty
        cards.Clear();
    }
    public void DrawCard(CardUI card)
    {
        cards.Add(card);
        handAnimation.DrawAnimation(card.ThisTransform);
    }
    public void RemoveCard(CardUI card)
    {
        discardPile.AddCard(card.Data);
        handAnimation.DiscardAnimation(card.ThisTransform);
        cards.Remove(card);
        handAnimation.FitCards();
    }

    #endregion
}

