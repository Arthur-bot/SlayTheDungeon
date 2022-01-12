using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int damage;
    private bool alreadyTriggered;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && !alreadyTriggered)
        {
            alreadyTriggered = true;
            LootManager.Instance.SetupLoop(damage);
            GameManager.Instance.Player.Controller.IsMoving = false;
        }
    }
}
