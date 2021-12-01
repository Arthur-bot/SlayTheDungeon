using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] private CardLoot lootCardPrefab;
    [SerializeField] private Pile playerDeck;
    [SerializeField] private GameObject lootPanel;
    [SerializeField] private PlayerData playerData;
    private CardDataBase dataBase;
    private int nbOfChoices = 3;
    private List<CardLoot> loots = new List<CardLoot>();
    private bool isLooting = false;

    public bool IsLooting { get => isLooting; set => isLooting = value; }

    private void Start()
    {
        dataBase = CardDataBase.Instance;
    }

    public void SetupLoop()
    {
        isLooting = true;
        lootPanel.SetActive(true);
        while (loots.Count < nbOfChoices)
        {
            loots.Add(Instantiate(lootCardPrefab, lootPanel.transform));
        }
        List<CardData> newLoots = dataBase.PickRandomCards(nbOfChoices);
        for (int i = 0; i < nbOfChoices; i++)
        {
            loots[i].GetComponent<CardUI>().SetupCard(newLoots[i]);
        }
    }

    public void PickLoot(CardLoot loot)
    {
        loot.transform.SetParent(transform, true);
        CardAnimation.Instance.GetLootAnimation(loot.ThisTransform);
        loots.Remove(loot);
        playerDeck.AddCard(loot.GetComponent<CardUI>().Data);
        playerData.AddCard(loot.GetComponent<CardUI>().Data);
        lootPanel.SetActive(false);
        isLooting = false;
    }
}
