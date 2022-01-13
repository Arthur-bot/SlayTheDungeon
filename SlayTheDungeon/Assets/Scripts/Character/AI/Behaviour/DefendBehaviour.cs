using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBehaviour : BaseBehaviour
{
    public DefendBehaviour()
    {
        preferedKeywords = new List<KeyWord> { KeyWord.Defend};
    }
}
