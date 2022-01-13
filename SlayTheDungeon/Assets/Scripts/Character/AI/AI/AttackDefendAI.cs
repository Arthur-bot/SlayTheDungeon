using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDefendAI", menuName = "AI")]
public class AttackDefendAI : BaseAI
{
    private AttackBehaviour attackBehaviour = new AttackBehaviour();
    private DefendBehaviour defendBehaviour = new DefendBehaviour();
    public override void Init()
    {
        attackBehaviour.Owner = Owner;
        defendBehaviour.Owner = Owner;
        UpdateBehaviour();
    }

    public override void UpdateBehaviour()
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
