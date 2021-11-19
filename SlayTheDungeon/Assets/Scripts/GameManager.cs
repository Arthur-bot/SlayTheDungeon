using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    [SerializeField] private CharacterData player;
    [SerializeField] private List<Enemy> enemies;

    private DeckPile playerDeck;
    private bool isPlayerTurn;
    private Hand hand;


    #endregion

    #region Properties

    public bool IsPlayerTurn { get => isPlayerTurn; set => isPlayerTurn = value; }

    public List<Enemy> Enemies => enemies;

    public CharacterData Player => player;

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();
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

        // Start combat 
        EndTurn();
    }

    #endregion

    #region Public Methods

    public void EndTurn()
    {
        if (isPlayerTurn) // End of player's turn
        {
            isPlayerTurn = false;
            hand.DiscardHand();
            StartCoroutine(EnnemyTurn());
        }
        else // Start of player's turn
        {
            StartCoroutine(DrawHand());
            isPlayerTurn = true;
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator EnnemyTurn()
    {
        foreach (Enemy monster in enemies)
        {
            monster.Attack();
            yield return new WaitForSeconds(1.0f);
        }

        EndTurn();
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
