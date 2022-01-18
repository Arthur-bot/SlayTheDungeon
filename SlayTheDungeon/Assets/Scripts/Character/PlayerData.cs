using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{
    #region Fields

    private List<CardData> deck = new List<CardData>();
    [SerializeField] private List<CardData> startingDeck;
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
        foreach (CardData card in startingDeck)
        {
            deck.Add(Instantiate(card));
        }
        Controller = GetComponent<PlayerController>();
    }

    protected override void Update()
    {
        if (IsAlive && stats.CurrentHealth <= 0)
        {
            GameManager.Instance.EndGame();
        }
    }
    #endregion

    #region Public Methods

    public override void DrawCards(int value)
    {
        GameManager.Instance.DrawCards(value);
    }

    public override void AddCards(List<CardData> toAdd)
    {
        GameManager.Instance.AddCards(toAdd);
    }
    public override void GetEnergy(int value)
    {
        ChangeEnergy(value);
    }
    public bool CanPlayCard(CardData card) => CurrentEnergy - card.Cost >= 0;

    public void ChangeEnergy(int amount)
    {
        CurrentEnergy += amount;
        GameUI.Instance.EnergyText.text = CurrentEnergy + "/" + energy;
    }
    public void AddCard(CardData card)
    {
        deck.Add(Instantiate(card));
    }

    public void RemoveCard(CardData card)
    {
        deck.Remove(card);
    }

    public void ResetEnergy()
    {
        CurrentEnergy = energy;
        GameUI.Instance.EnergyText.text = CurrentEnergy + "/" + energy;
    }
    public void Rest()
    {
        TakeDamage(-stats.BaseStats.Health / 2);
    }
    public void Forge()
    {
        foreach (CardData card in deck)
        {
            if (card.LimitedUse)
                card.NbUse++;
        }
    }

    #endregion
}
