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

    public Combinations(EnnemyData enemy, int level)
    {
        this.ennemies = new List<EnnemyData>() { enemy };
        this.level = level;
        this.boss = false;
    }
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
    [SerializeField] private Sprite drainIcon;

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
    public Sprite DrainIcon => drainIcon;

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();

        pathToMods = Application.dataPath + "/Mods";
        if (!Directory.Exists(pathToMods))
        {
            Directory.CreateDirectory(pathToMods);
            Directory.CreateDirectory(pathToMods + "/Cards");
            Directory.CreateDirectory(pathToMods + "/Monsters");
            Directory.CreateDirectory(pathToMods + "/Effects");
        }

        ChargeModdedEffect("Effects");
        ChargeModdedCards("Cards");
        ChargeModdedMonsters("Monsters");

        for (int i = 0; i < 100; i++)
        {
            PickRandomEnnemyCombination(1);
        }
    }

    #endregion

    #region Private Methods

    private void ChargeModdedCards(string nameOfSet)
    {
        if (!Directory.Exists(pathToMods + "/" + nameOfSet)) return;

        // Get all JSON files
        ListOfJSON = Directory.GetFiles(pathToMods + "/" + nameOfSet, "*.json");

        for (var i = 0; i < ListOfJSON.Length; i++)
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

    }

    private void ChargeModdedMonsters(string nameOfSet)
    {
        if (!Directory.Exists(pathToMods + "/" + nameOfSet)) return;

        //Get all JSON files
        ListOfJSON = Directory.GetFiles(pathToMods + "/" + nameOfSet, "*.json");

        for (var i = 0; i < ListOfJSON.Length; i++)
        {
            // Get data from JSON
            StreamReader reader = new StreamReader(ListOfJSON[i]);
            var jsonString = reader.ReadToEnd();
            var monsterData = new MonsterStructure();
            JsonUtility.FromJsonOverwrite(jsonString, monsterData);

            // Copy data in a new scriptable object
            var monster = ScriptableObject.CreateInstance<EnnemyData>();
            monster.name = monsterData.name;
            monster.MonsterFormStructure(monsterData);

            var combination = new Combinations(
                monster,
                monsterData.difficulty
            );

            ennemyCombos.Add(combination);
        }
    }

    private void ChargeModdedEffect(string nameOfSet)
    {
        if (!Directory.Exists(pathToMods + "/" + nameOfSet)) return;

        //Get all JSON files
        ListOfJSON = Directory.GetFiles(pathToMods + "/" + nameOfSet, "*.json");

        for (var i = 0; i < ListOfJSON.Length; i++)
        {
            // Get data from JSON
            StreamReader reader = new StreamReader(ListOfJSON[i]);
            var jsonString = reader.ReadToEnd();
            var effectData = new ConditionalEffectStructure();
            JsonUtility.FromJsonOverwrite(jsonString, effectData);

            // Copy data in a new scriptable object
            var effect = ScriptableObject.CreateInstance<ConditionnalEffect>();
            effect.name = effectData.name;
            AssetDatabase.CreateAsset(effect, "Assets/Mods/" + effect.name + ".asset");
            effect.EffectFromJson(effectData);
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
