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

    public PlayerController Controller { get; private set; }

    #endregion

    #region Protected Methods

    protected override void Awake()
    {
        base.Awake();

        Controller = GetComponent<PlayerController>();
    }

    #endregion

    #region Public Methods

    public bool CanPlayCard(CardData card) => CurrentEnergy >= card.Cost;

    public void PlayCard(CardData card)
    {
        CurrentEnergy -= card.Cost;
        GameUI.Instance.EnergyText.text = CurrentEnergy + "/" + energy;
    }
    public void AddCard(CardData card)
    {
        deck.Add(card);
    }

    public void ResetEnergy()
    {
        CurrentEnergy = energy;
        GameUI.Instance.EnergyText.text = CurrentEnergy + "/" + energy;
    }

    #endregion
}
