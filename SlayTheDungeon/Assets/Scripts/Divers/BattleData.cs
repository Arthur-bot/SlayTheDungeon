using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    private GameManager gameManager;
    private int nbPlayedCard;

    public int NbPlayedCard { get => nbPlayedCard; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnEndTurn += ResetCounter;
    }

    private void ResetCounter(object sender, EventArgs e)
    {
        nbPlayedCard = 0;
    }
    public void IncrementNbPlayedCard()
    {
        nbPlayedCard++;
    }
}
