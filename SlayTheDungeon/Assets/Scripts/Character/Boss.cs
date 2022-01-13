using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum KeyWord
{
    Attack,
    Defend
}

public class Boss : Enemy
{
    private int energy = 3;
    private BaseAI ai;
    private List<CardData> deck = new List<CardData>();
    private List<CardData> discard = new List<CardData>();
    private List<CardData> hand = new List<CardData>();
    private int damageTaken = 0;

    public List<CardData> Hand { get => hand; set => hand = value; }
    public int DamageTaken { get => damageTaken; }

    public override void SetupEnemy()
    {
        base.SetupEnemy();
        foreach(CardData card in EnnemyData.EnnemyDeck)
        {
            deck.Add(Instantiate(card));
        }
        ai = Instantiate(EnnemyData.Ai);
        ai.Owner = this;
        Debug.Log(ai.Owner);
        ai.Init();
        DrawCards(5);
    }
    public override void PlayTurn()
    {
        //information collecting
        
        //decision taking
        energy = 3;

        //setup target to use cards properly
        TargetingSystem.Instance.SetTarget(this);

        CardData toPlay = ai.LookForCard();
        if (toPlay != null)
            PlayACard(toPlay);

        // end the turn
        damageTaken = 0;
        ai.UpdateBehaviour();
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

    public override void TakeDamage(int amount)
    {
        stats.Damage(amount);
        damageTaken += amount;

        GameManager.Instance.Shake(0.1f, 0.1f);
    }
}
