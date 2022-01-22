using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    private GameManager gameManager;
    private int nbPlayedCard;
    private Target currentTarget;
    public int NbPlayedCard { get => nbPlayedCard; }
    public Target CurrentTarget { get => currentTarget; set => currentTarget = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnEndTurn += EndTurn;
    }

    private void EndTurn(object sender, EventArgs e)
    {
        ResetCounter();
    }

    public void Reset()
    {
        ResetCounter();
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
