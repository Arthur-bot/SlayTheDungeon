using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{
    #region Fields

    [SerializeField] private List<CardData> deck;
    [SerializeField] private int energy;

    #endregion

    #region Properties

    public List<CardData> Deck => deck;

    public int CurrentEnergy { get; private set; }

    #endregion

    #region Public Methods

    public bool CanPlayCard(CardData card) => CurrentEnergy >= card.Cost;

    public void PlayCard(CardData card)
    {
        CurrentEnergy -= card.Cost;
    }

    public void ResetEnergy()
    {
        CurrentEnergy = energy;
    }

    #endregion
}