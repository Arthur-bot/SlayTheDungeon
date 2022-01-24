using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour
{
    [SerializeField] private int nbOfChoice;
    private bool isOpen;

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            isOpen = true;
            LootManager.Instance.SetupLoop(nbOfChoice);
            (GameManager.Instance.CurrentRoom as Room).LinkedDungeonElement.Clean();
        }
    }
}
