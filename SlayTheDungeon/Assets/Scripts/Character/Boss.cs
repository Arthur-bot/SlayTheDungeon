using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : Enemy
{
    private int energy = 3;
    private List<CardData> deck = new List<CardData>();
    private List<CardData> discard = new List<CardData>();
    private List<CardData> hand = new List<CardData>();

    public override void SetupEnemy()
    {
        base.SetupEnemy();
        foreach(CardData card in EnnemyData.EnnemyDeck)
        {
            deck.Add(Instantiate(card));
        }
        DrawCards(5);
    }
    public override void PlayTurn()
    {
        energy = 3;
        TargetingSystem.Instance.SetTarget(this);
        PlayACard(hand[0]);
        DiscardHand();
        DrawCards(5);
    }

    private void DiscardHand()
    {
        foreach (CardData card in hand)
        {
            discard.Add(card);
        }
        hand.Clear();
    }

    private void PlayACard(CardData card)
    {
        if(energy > card.Cost)
        {
            energy -= card.Cost;
            card.Use(false);
            hand.Remove(card);
            if (!card.LimitedUse || card.NbUse > 0)
                discard.Add(card);
        }
    }
    private void DrawOneCard()
    {
        if (deck.Count == 0)
        {
            Shuffle();
        }
        int randomIndex = Random.Range(0, deck.Count);
        CardData selected = deck[randomIndex];
        deck.RemoveAt(randomIndex);
        hand.Add(selected);
    }

    private void Shuffle()
    {
        foreach(CardData card in discard)
        {
            deck.Add(card);
        }
        discard.Clear();
    }

    public override void DrawCards(int value)
    {
        for (int i = 0; i < value; i++)
        {
            DrawOneCard();
        }
    }
    public override void GetEnergy(int value)
    {
        energy += value;
    }
}
