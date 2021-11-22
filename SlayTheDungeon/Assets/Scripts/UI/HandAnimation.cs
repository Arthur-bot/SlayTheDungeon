using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandAnimation : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform deckPile;
    [SerializeField] private Transform discardPile;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float gapBetweenCard;
    [SerializeField] private Vector2 handCenter;

    private List<RectTransform> cards;
    private int howManyCard;
    private Vector2 memoryPosition;

    #endregion

    #region Public Methods

    // Public Functions
    public void FitCards()
    {
        // Replaces correctly all the cards already in hand
        Vector2 end = handCenter - new Vector2((howManyCard - 1) * gapBetweenCard / 2, 0);
        for (int i = 0; i < howManyCard; i++)
        {
            Debug.Log(end);
            cards[i].DOAnchorPos(end, 0.2f).OnComplete(cards[i].GetComponent<CardUI>().InitPosition);
            end += new Vector2(gapBetweenCard, 0);
        }
    }
    public void MemorizeCardGeometry(Vector2 position)
    {
        memoryPosition = position;
    }
    public void DrawAnimation(RectTransform card)
    {
        howManyCard++;
        // Animates the size
        card.localScale = Vector2.zero;
        card.DOScale(new Vector2(1f, 1f), 0.3f);
        // Animates the trajectory
        card.position = deckPile.transform.position; // start point
        // Computes the end position of the card
        //Vector3 end = handCenter + new Vector2(howManyCard * gapBetweenCard / 2, 0);
        //card.DOAnchorPos(end, 0.3f);
        FitCards();
    }
    public void DiscardAnimation(RectTransform card)
    {
        card.DORotate(new Vector3(0, 0, 540), 0.4f);
        card.DOScale(new Vector2(0.1f, 0.1f), 0.4f);
        StartCoroutine(DiscardMove(card));
        RemoveCard();
    }
    public void ReturnHandAnimation(RectTransform card)
    {
        card.DOAnchorPos(memoryPosition, 0.2f);
    }
    public void RemoveCard()
    {
        howManyCard--;
    }

    // Setter
    public void SetCards(List<RectTransform> toSet)
    {
        cards = toSet;
    }

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        howManyCard = 0;
    }

    #endregion

    #region Private Methods

    private IEnumerator DiscardMove(RectTransform card)
    {
        float time = 0f;
        float duration = Random.Range(0.4f, 0.7f);
        Vector2 start = card.position;
        Vector2 end = discardPile.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTimeOnCurve = time / duration;
            float yValueOfCurve = animationCurve.Evaluate(normalizedTimeOnCurve);
            card.position = Vector2.Lerp(start, end, normalizedTimeOnCurve) + new Vector2(0f, yValueOfCurve);
            yield return null;
        }
        Destroy(card.gameObject);
    }

    #endregion
}
