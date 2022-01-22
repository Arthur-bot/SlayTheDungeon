using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    private GameManager gameManager;
    private int nbPlayedCard;
    private bool playerTurn;
    public int NbPlayedCard { get => nbPlayedCard; }
    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnEndTurn += EndTurn;
        playerTurn = true;
    }

    private void EndTurn(object sender, EventArgs e)
    {
        ResetCounter();
        playerTurn = !playerTurn;
        Debug.Log(playerTurn);
    }

    public void Reset()
    {
        ResetCounter();
        playerTurn = true;
    }

    private void ResetCounter()
    {
        nbPlayedCard = 0;
    }
    public void IncrementNbPlayedCard()
    {
        nbPlayedCard++;
    }
}
