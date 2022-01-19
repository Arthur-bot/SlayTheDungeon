using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : ScriptableObject
{
    protected BaseBehaviour currentBehaviour;
    private Boss owner;

    public Boss Owner { get => owner; set => owner = value; }

    public abstract void  Init();
    public abstract void TakeDecision();
    public List<CardData> LookForCards()
    {
        return currentBehaviour.LookForCards();
    }
    protected bool CheckCard(KeyWord keyword)
    {
        foreach (CardData card in Owner.Hand)
        {
            if (card.Keywords.Contains(keyword))
            {
                return true;
            }
        }
        return false;
    }
}
