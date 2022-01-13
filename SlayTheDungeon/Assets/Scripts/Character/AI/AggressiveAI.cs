using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveAI : BaseAI
{
    // Start is called before the first frame update
    override protected void Start()
    {
        this.preferedKeywords.Add(KeyWord.Attack);
        owner = this.GetComponent<Boss>();
    }
}
