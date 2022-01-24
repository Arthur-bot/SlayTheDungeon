using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

    public void ResetDeckPile(List<CardData> cardsList)
    {
        cards.Clear();
        cards = new List<CardData>(cardsList);

        Shuffle();

        countText.text = cards.Count.ToString();
    }

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
        else if (discardPile.Cards.Count > 0)
        {
            discardPile.Shuffle(this);
            Shuffle();
            DrawCard();
        }
    }
    public void DrawCard(CardData toDraw)
    {
        var newCard = Instantiate(cardTemplate, hand.transform);
        // Initializes it whit the random card
        newCard.SetupCard(Instantiate(toDraw));
        // Adds the card to the hand
        hand.DrawCard(newCard.GetComponent<CardUI>());
        audioManager.PlaySFX(audioDraw);
    }

    #endregion

    #region Private Methods

    private void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var random = Random.Range(1, cards.Count);
            cards[i] = cards[random];
            cards[random] = card;
        }
    }

    #endregion
}
