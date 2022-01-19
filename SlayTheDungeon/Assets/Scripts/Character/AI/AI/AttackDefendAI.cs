using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDefendAI : BaseAI
{
    private BaseBehaviour attackBehaviour = new BaseBehaviour();
    private BaseBehaviour defendBehaviour = new BaseBehaviour();
    public override void Init()
    {
        attackBehaviour.Owner = Owner;
        attackBehaviour.PreferedKeywords = new List<KeyWord> { KeyWord.Attack, KeyWord.Poison };
        defendBehaviour.Owner = Owner;
        defendBehaviour.PreferedKeywords = new List<KeyWord> { KeyWord.Defend };
        UpdateBehaviour();
    }

    public override void TakeDecision()
    {
        Owner.StartCoroutine(TakeDecisionInTime());
    }

    IEnumerator TakeDecisionInTime()
    {
        bool playSpecificCard = true;
        List<CardData> cardsToPay = LookForCards();
        while (playSpecificCard)
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
            playSpecificCard = hasPlayedCard;
        }
        while (!playSpecificCard)
        {
            bool hasPlayedCard = false;
            foreach (CardData card in Owner.Hand)
            {
                if (Owner.PlayACard(card))
                {
                    yield return new WaitForSeconds(1.5f);
                    hasPlayedCard = true;
                    break;
                }
            }
            playSpecificCard = !hasPlayedCard;
        }
        UpdateBehaviour();
        Owner.IsPlaying = false;
    }

    public void UpdateBehaviour()
    {
        int randomNbr = Random.Range(0, 2);
        if (randomNbr == 0)
        {
            Debug.Log("attack behaviour");
            currentBehaviour = attackBehaviour;
        }
        else
        {
            Debug.Log("defend behaviour");
            currentBehaviour = defendBehaviour;
        }
    }
}
