using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    private TrapManager trapManager;
    protected bool alreadyTriggered;
    [SerializeField] private Sprite trapSprite;

    public Sprite TrapSprite { get => trapSprite; set => trapSprite = value; }

    private void Start()
    {
        trapManager = TrapManager.Instance;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() && !alreadyTriggered)
        {
            trapManager.SetupTrapPanel(this);
        }
    }

    public abstract void TriggerTrap(PlayerData target);
}
