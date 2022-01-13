using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveAI : BaseAI
{
    // Start is called before the first frame update
    override protected void Start()
    {
        this.preferedKeywords.Add(KeyWord.Defend);
        owner = this.GetComponent<Boss>();
    }
}
