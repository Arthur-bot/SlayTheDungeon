using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    #region Fields

    [SerializeField] private Hand playerHand;
    [SerializeField] private HandAnimation handAnimation;
    [SerializeField] private DeckPile playerDeck;
    [SerializeField] private Button endTurnButton;

    #endregion

    #region Properties

    public Hand PlayerHand => playerHand;

    public HandAnimation HandAnimation => handAnimation;

    public DeckPile PlayerDeck => playerDeck;

    public Button EndTurnButton => endTurnButton;

    #endregion
}
