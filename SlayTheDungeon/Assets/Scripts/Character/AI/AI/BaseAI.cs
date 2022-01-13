using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : ScriptableObject
{
    protected BaseBehaviour currentBehaviour;
    private Boss owner;

    public Boss Owner { get => owner; set => owner = value; }

    public abstract void  Init();
    public abstract void UpdateBehaviour();
    public CardData LookForCard()
    {
        return currentBehaviour.LookForCard();
    }
}
