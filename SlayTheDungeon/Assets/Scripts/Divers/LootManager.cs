using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] private CardLoot lootCardPrefab;
    [SerializeField] private Pile playerDeck;
    [SerializeField] private GameObject lootPanel;
    [SerializeField] private Transform lootCardContainers;
    private PlayerData playerData;
    private DataBase dataBase;
    private List<CardLoot> loots = new List<CardLoot>();
    private bool isLooting = false;

    public bool IsLooting { get => isLooting; set => isLooting = value; }

    private void Start()
    {
        dataBase = DataBase.Instance;
        playerData = GameManager.Instance.Player;
    }

    public void SetupLoop(int nbOfChoices)
    {
        isLooting = true;
        lootPanel.SetActive(true);
        while (loots.Count < nbOfChoices)
        {
            loots.Add(Instantiate(lootCardPrefab, lootCardContainers));
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
        playerData.AddCard(loot.GetComponent<CardUI>().Data);
        playerDeck.UpdateUI();
        lootPanel.SetActive(false);
        isLooting = false;
    }
    public void Skip()
    {
        lootPanel.SetActive(false);
        isLooting = false;
    }
}
