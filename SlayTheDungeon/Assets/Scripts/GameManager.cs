using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    private DeckPile playerDeck;
    private bool isPlayerTurn;
    private Hand hand;
    private List<Enemy> enemies;

    #endregion

    #region Properties

    public bool IsPlayerTurn { get => isPlayerTurn; set => isPlayerTurn = value; }

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();

        enemies = new List<Enemy>();
    }

    protected void Start()
    {
        if (GameUI.HasInstance)
        {
            var gameUI = GameUI.Instance;

            hand = gameUI.PlayerHand;
            playerDeck = gameUI.PlayerDeck;
            gameUI.EndTurnButton.onClick.AddListener(EndTurn);
        }
    }

    #endregion

    #region Public Methods

    public void EndTurn()
    {
        if (isPlayerTurn) // End of player's turn
        {
            hand.DiscardHand();
            EnnemyTurn();
            isPlayerTurn = false;
        }
        else // Start of player's turn
        {
            StartCoroutine(DrawHand());
            isPlayerTurn = true;
        }
    }

    #endregion

    #region Private Methods

    private void EnnemyTurn()
    {
        foreach (Enemy monster in enemies)
        {
            monster.Attack();
        }
    }
    private IEnumerator DrawHand()
    {
        for (int i = 0; i < 5; i++)
        {
            playerDeck.DrawCard();
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion
}
