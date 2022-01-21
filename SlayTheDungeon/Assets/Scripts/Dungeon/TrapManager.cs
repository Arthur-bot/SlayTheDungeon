using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapManager : Singleton<TrapManager>
{
    private PlayerController playerController;
    private PlayerData player;
    private Trap currentTrap;
    [SerializeField] private GameObject panel;
    [SerializeField] private CanvasGroup trapButton;

    private void Start()
    {
        playerController = GameManager.Instance.Player.Controller;
        player = GameManager.Instance.Player;
    }
    public Trap CurrentTrap { get => currentTrap; set => currentTrap = value; }

    public void SetupTrapPanel(Trap toTrigger)
    {
        playerController.IsMoving = false;
        panel.SetActive(true);
        trapButton.interactable = player.Deck.Count > 5;
        trapButton.alpha = player.Deck.Count > 5 ? 1f : 0.75f;
        currentTrap = toTrigger;
    }

    public void TrapClick()
    {
        currentTrap.AlreadyTriggered = true;
        var randomCard = player.Deck[Random.Range(0, player.Deck.Count)];
        player.RemoveCard(randomCard);
        panel.SetActive(false);
        playerController.IsMoving = true;
    }

    public void DamageClick()
    {
        currentTrap.AlreadyTriggered = true;
        player.TakeDamage(3);
        panel.SetActive(false);
        playerController.IsMoving = true;
    }

    public void BackClick()
    {
        panel.SetActive(false);
        playerController.Reverse();
        playerController.IsMoving = true;
    }
}
