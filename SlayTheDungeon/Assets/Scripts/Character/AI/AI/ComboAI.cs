using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAI : BaseAI
{
    private List<CardData> combosCards = new List<CardData>();
    private List<CardData> eliminatedCards = new List<CardData>();
    private DrawBehaviour drawBehaviour = new DrawBehaviour();
    private MinCostBehaviour costBehaviour = new MinCostBehaviour();
    private BaseBehaviour baseBehaviour = new BaseBehaviour();
    public override void Init()
    {
        baseBehaviour.Owner = Owner;
        costBehaviour.Owner = Owner;
        drawBehaviour.Owner = Owner;

        currentBehaviour = drawBehaviour;
    }

    public override void TakeDecision()
    {
        Owner.StartCoroutine(TakeDecisionInTime());
    }

    IEnumerator TakeDecisionInTime()
    {
        CleanCard();
        // Play all the cards to draw for a few energy
        drawBehaviour.FreeDraw = true;
        currentBehaviour = drawBehaviour;
        List<CardData> cardsToPay = LookForCards();
        bool canPlay = true;
        while (canPlay)
        {
            bool hasPlayedCard = false;
            foreach (CardData card in cardsToPay)
            {
                if (Owner.PlayACard(card))
                {
                    yield return new WaitForSeconds(1.5f);
                    hasPlayedCard = true;
                    cardsToPay.Remove(card);
                    break;
                }
            }
            canPlay = hasPlayedCard;
        }
        if (CheckCard(KeyWord.Combo))
        {
            // Play generation and energy card
            baseBehaviour.PreferedKeywords = new List<KeyWord> {KeyWord.Generation, KeyWord.Energy };
            currentBehaviour = baseBehaviour;
            canPlay = true;
            cardsToPay = LookForCards();
            while (canPlay)
            {
                bool hasPlayedCard = false;
                foreach (CardData card in cardsToPay)
                {
                    if (Owner.PlayACard(card))
                    {
                        yield return new WaitForSeconds(1.5f);
                        hasPlayedCard = true;
                        cardsToPay.Remove(card);
                        break;
                    }
                }
                CleanCard();
                canPlay = hasPlayedCard;
            }
            //Play  a maximum amount of cards
            int toKeep = FindComboCost();
            costBehaviour.LimitCost = Owner.Energy - toKeep;
            currentBehaviour = costBehaviour;
            canPlay = true;
            cardsToPay = LookForCards();
            while (canPlay)
            {
                bool hasPlayedCard = false;
                foreach (CardData card in cardsToPay)
                {
                    if (Owner.PlayACard(card))
                    {
                        yield return new WaitForSeconds(1.5f);
                        hasPlayedCard = true;
                        cardsToPay.Remove(card);
                        break;
                    }
                }
                costBehaviour.LimitCost = Owner.Energy - toKeep;
                CleanCard();
                cardsToPay = LookForCards();
                canPlay = hasPlayedCard;
            }
            //Play the combo cards
            foreach(CardData card in combosCards)
            {
                Owner.Hand.Add(card);
                Owner.PlayACard(card);
                yield return new WaitForSeconds(1.5f);
            }
            combosCards.Clear();
        }
        else
        {
            //Tempo with defense and heal cards
            baseBehaviour.PreferedKeywords = new List<KeyWord> { KeyWord.Defend, KeyWord.Heal };
            currentBehaviour = baseBehaviour;
            canPlay = true;
            cardsToPay = LookForCards();
            while (canPlay)
            {
                bool hasPlayedCard = false;
                foreach (CardData card in cardsToPay)
                {
                    if (Owner.PlayACard(card))
                    {
                        yield return new WaitForSeconds(1.5f);
                        hasPlayedCard = true;
                        cardsToPay.Remove(card);
                        break;
                    }
                }
                canPlay = hasPlayedCard;
            }
            //Remained energy with attack cards
            baseBehaviour.PreferedKeywords = new List<KeyWord> { KeyWord.Attack };
            canPlay = true;
            cardsToPay = LookForCards();
            while (canPlay)
            {
                bool hasPlayedCard = false;
                foreach (CardData card in cardsToPay)
                {
                    if (Owner.PlayACard(card))
                    {
                        yield return new WaitForSeconds(1.5f);
                        hasPlayedCard = true;
                        cardsToPay.Remove(card);
                        break;
                    }
                }
                canPlay = hasPlayedCard;
            }
        }
        GetCardBack();
        Owner.IsPlaying = false;
    }

    private int FindComboCost()
    {
        int cost = 0;
        for (int i = Owner.Hand.Count - 1; i >= 0; i--)
        {
            if (Owner.Hand[i].Keywords.Contains(KeyWord.Combo))
            {
                cost += Owner.Hand[i].Cost;
                combosCards.Add(Owner.Hand[i]);
                Owner.Hand.RemoveAt(i);
            }
        }
        return cost;
    }

    private void CleanCard()
    {
        if (Owner.Stats.CurrentHealth <= Owner.Stats.BaseStats.Health / 2)
        {
            for (int i = Owner.Hand.Count - 1; i >= 0; i--)
            {
                if (Owner.Hand[i].Keywords.Contains(KeyWord.PayLife))
                {
                    eliminatedCards.Add(Owner.Hand[i]);
                    Owner.Hand.RemoveAt(i);
                }
            }
        }
    }
    private void GetCardBack()
    {
        foreach (CardData card in eliminatedCards)
        {
            Owner.Hand.Add(card);
        }
    }
}
