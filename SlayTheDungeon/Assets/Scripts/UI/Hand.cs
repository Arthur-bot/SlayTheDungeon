using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    #region Fields

    [SerializeField] private Pile discardPile;
    private CardAnimation handAnimation;
    private RectTransform thisTransform;
    private List<CardUI> cards;
    private GameManager gameManager;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        handAnimation = CardAnimation.Instance;
        cards = new List<CardUI>();
        thisTransform = GetComponent<RectTransform>();
        handAnimation.SetCards(cards);
        gameManager = GameManager.Instance;
    }

    #endregion

    #region Public Methods

    public void DiscardHand()
    {
        if (cards == null) return;
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
        if (card.Data.NbUse > 0 || !card.Data.LimitedUse)
        {
            discardPile.AddCard(card.Data);
            handAnimation.DiscardAnimation(card.ThisTransform);
            cards.Remove(card);
            handAnimation.FitCards();
        }
        else
        {
            handAnimation.RemoveCard();
            cards.Remove(card);
            handAnimation.FitCards();
            gameManager.Player.RemoveCard(card.Data);
            Destroy(card.gameObject);
        }

    }

    #endregion
}

