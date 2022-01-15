using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBehaviour : BaseBehaviour
{
    protected int defenseCount/*, defenseMean, passedTurns*/;

    public DefendBehaviour()
    {
        preferedKeywords = new List<KeyWord> { KeyWord.Defend};
    }
}
