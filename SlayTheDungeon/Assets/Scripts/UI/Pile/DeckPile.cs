using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckPile : Pile
{
    #region Fields

    [SerializeField] private DiscardPile discardPile;
    [SerializeField] private AudioClip audioDraw;
    private AudioManager audioManager;

    private Hand hand;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        hand = GameUI.Instance.PlayerHand;
        countText.text = cards.Count.ToString();
    }
    protected void Start()
    {
        audioManager = AudioManager.Instance;
    }

    #endregion

    #region Public Methods

    public void DrawCard()
    {
        if (cards.Count > 0)
        {
            // Chooses a random number which corresponds to a card
            int randomIndex = Random.Range(0, cards.Count);
            // Creates a new empty card
            var newCard = Instantiate(cardTemplate, hand.transform);
            // Initializes it whit the random card
            newCard.SetupCard(cards[randomIndex]);
            // Adds the card to the hand
            hand.DrawCard(newCard.GetComponent<CardUI>());
            audioManager.PlaySFX(audioDraw);
            // Removes the card from the pile
            cards.RemoveAt(randomIndex);

            countText.text = cards.Count.ToString();
        }
        else
        {
            discardPile.Shuffle(this);
            DrawCard();
        }
    }

    #endregion
}
