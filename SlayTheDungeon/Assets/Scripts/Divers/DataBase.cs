using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Combinations
{
    public List<EnnemyData> ennemies;
    public int level;
}
 
public class DataBase : Singleton<DataBase>
{

    [SerializeField] private List<CardData> allCards;
    [SerializeField] private List<Combinations> ennemyCombos;

    private void Start()
    {
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

    public List<EnnemyData> PickRandomEnnemyCombination(int level)
    {
        List<Combinations> combinationOfExpectedLevel = ennemyCombos.Where(x => x.level == level).ToList();
        List<EnnemyData> newPick = combinationOfExpectedLevel[Random.Range(0, combinationOfExpectedLevel.Count - 1)].ennemies;
        return newPick;
    }
}
