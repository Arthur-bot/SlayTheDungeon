using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    #region Fields

    private HandAnimation handAnimation;
    private RectTransform thisTransform;
    private List<RectTransform> cards;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        handAnimation = GetComponent<HandAnimation>();
        cards = new List<RectTransform>();
        thisTransform = GetComponent<RectTransform>();
        handAnimation.SetCards(cards);
    }

    #endregion

    #region Public Methods

    public void DiscardHand()
    {
        // Discards all cards
        foreach (RectTransform card in cards)
        {
            handAnimation.DiscardAnimation(card);
        }
        // The hand is empty
        cards.Clear();
    }
    public void DrawCard(RectTransform card)
    {
        cards.Add(card);
        handAnimation.DrawAnimation(card);
    }
    public void RemoveCard(RectTransform card)
    {
        handAnimation.DiscardAnimation(card);
        cards.Remove(card);
        handAnimation.FitCards();
    }

    #endregion
}

