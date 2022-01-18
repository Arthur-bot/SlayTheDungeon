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
    [SerializeField] private Image trapImage;

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
        trapImage.sprite = toTrigger.TrapSprite;
        currentTrap = toTrigger;
    }

    public void TrapClick()
    {
        currentTrap.TriggerTrap(player);
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
