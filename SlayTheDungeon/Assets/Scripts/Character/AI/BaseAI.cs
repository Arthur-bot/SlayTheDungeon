using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected List<int> preferedKeywords;
    private Boss owner;
        
    // Start is called before the first frame update, during this, we define prefered keywords and the data we wish to use to make decisions
    void Start()
    {
        owner = this.GetComponent<Boss>();
    }

    protected void TakeDecision()
    {

    }

    // LookFor is the base function to use when looking for a specific card or effect
    private List<int> LookFor(int preferedKeyword)
    {
        var corresponding = new List<int>();
        if (preferedKeywords.Count > 0)
        {
            for (int i = 0; i < owner.hand.Count; i++)
            {
                if (owner.hand[i].keywords.Find(x => x == preferedKeyword) != 0 || owner.hand[i].keywords[0] == preferedKeyword)
                {
                    corresponding.Add(i);
                }
            }
        }
        else
        {
            corresponding.Add(0);
        }

        return corresponding;
    }
}
