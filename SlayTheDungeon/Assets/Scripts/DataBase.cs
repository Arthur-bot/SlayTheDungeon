using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : Singleton<DataBase>
{
    [SerializeField] private List<CardData> allCards;
    [SerializeField] private List<EnnemyData> allEnnemies;

    private void Start()
    {
        PickRandomCards(5);
    }

    public List<CardData> PickRandomCards(int number)
    {
        List<CardData> picks = new List<CardData>();
        for (int i = 0; i < number; i++)
        {
            CardData newPick = allCards[Random.Range(0, allCards.Count)];
            while (picks.Contains(newPick))
            {
                newPick = allCards[Random.Range(0, allCards.Count)];
            }
            picks.Add(newPick);
        }
        return picks;
    }

    public EnnemyData PickRandomEnnemy()
    {
        EnnemyData newPick = allEnnemies[Random.Range(0, allEnnemies.Count)];
        return newPick;
    }
}
