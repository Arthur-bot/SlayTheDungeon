using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour
{
    [SerializeField] private int nbOfChoice;
    private bool isOpen;

    private void OnMouseDown()
    {
        if (!isOpen && !GameManager.Instance.InBattle && !LootManager.Instance.IsLooting)
        {
            isOpen = true;
            LootManager.Instance.SetupLoop(nbOfChoice);
        }
    }
}
