using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeck : Pile
{
    private void Start()
    {
        cards = GameManager.Instance.Player.Deck;
    }
    private void Update()
    {
        UpdateUI();
    }
}
