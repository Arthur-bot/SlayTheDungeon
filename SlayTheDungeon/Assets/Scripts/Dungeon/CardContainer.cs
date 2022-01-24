using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour
{
    [SerializeField] private int nbOfChoice;
    private bool isOpen;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && !isOpen)
        {
            isOpen = true;
            LootManager.Instance.SetupLoop(nbOfChoice);
            GameManager.Instance.Player.Controller.IsMoving = false;
        }
    }
}
