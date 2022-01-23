using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Combinations
{
    public List<EnnemyData> ennemies;
    public int level;
    public bool boss;
}

public class DataBase : Singleton<DataBase>
{
    #region Fields

    [SerializeField] private List<CardData> allCards;
    [SerializeField] private List<Combinations> ennemyCombos;

    [Header("Effect Icon")] 
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite armorIcon;
    [SerializeField] private Sprite poisonIcon;
    [SerializeField] private Sprite furyIcon;
    [SerializeField] private Sprite dodgeIcon;

    [Header("Mod")] 
    [SerializeField] private string pathToMods;
    private string[] ListOfJSON;

    #endregion

    #region Properties

    public Sprite AttackIcon => attackIcon;

    public Sprite ArmorIcon => armorIcon;

    public Sprite PoisonIcon => poisonIcon;

    public Sprite FuryIcon => furyIcon;

    public Sprite DodgeIcon => dodgeIcon;

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();

        pathToMods = Application.dataPath + "/Mods";
        if (!Directory.Exists(pathToMods))
            Directory.CreateDirectory(pathToMods);
    }

    protected void Start()
    {
        ChargeSet("Test");
        for (int i = 0; i < 100; i++)
        {
            PickRandomEnnemyCombination(1);
        }
    }

    #endregion

    #region Private Methods

    private void ChargeSet(string nameOfSet)
    {
        if (!Directory.Exists(pathToMods + "/" + nameOfSet))
        {
            return;
        }
        else
        {
            ListOfJSON = Directory.GetFiles(pathToMods + "/" + nameOfSet, "*.json");

            for (int i = 0; i < ListOfJSON.Length; i++)
            {
                // Get data from JSON
                StreamReader reader = new StreamReader(ListOfJSON[i]);
                var jsonString = reader.ReadToEnd();
                var cardData = new CardStructure();
                JsonUtility.FromJsonOverwrite(jsonString, cardData);

                // Copy data in a new scriptable object
                var card = ScriptableObject.CreateInstance<CardData>();
                card.name = cardData.cardName;
                card.CardFromStructure(cardData);

                allCards.Add(card);
            }

            return;

        }

    }

    #endregion

    #region Public Methods

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
        List<Combinations> combinationOfExpectedLevel = ennemyCombos.Where(x => x.level == level && x.boss == false).ToList();
        Debug.Log(Random.Range(0, combinationOfExpectedLevel.Count));
        List<EnnemyData> newPick = combinationOfExpectedLevel[Random.Range(0, combinationOfExpectedLevel.Count)].ennemies;
        return newPick;
    }
    public List<EnnemyData> PickRandomBoss()
    {
        List<Combinations> combinationOfBoss = ennemyCombos.Where(x => x.boss == true).ToList();
        Debug.Log(Random.Range(0, combinationOfBoss.Count));
        List<EnnemyData> newPick = combinationOfBoss[Random.Range(0, combinationOfBoss.Count)].ennemies;
        return newPick;
    }

    #endregion
}
