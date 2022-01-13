using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : BaseBehaviour
{
    public AttackBehaviour ()
    {
        preferedKeywords = new List<KeyWord> { KeyWord.Attack };
    }
}
