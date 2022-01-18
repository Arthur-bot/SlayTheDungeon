using System;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData : MonoBehaviour
{
    #region Fields

    [Header("Stats")]
    [SerializeField] protected StatSystem stats;

    [Header("UI")] 
    [SerializeField] protected HUD hud;

    [Header("Materials")]
    [SerializeField] private Material baseMaterial;

    [SerializeField] private Material outlineMaterial;

    #endregion

    #region Properties

    public StatSystem Stats => stats;

    public bool IsAlive { get; private set; } = true;

    public bool IsTargeted
    {
        get => hud.CharacterSprite.material == outlineMaterial;
        set
        {
            if (value == IsTargeted) return;

            hud.CharacterSprite.material = value ? outlineMaterial : baseMaterial;
        }
    }

    #endregion

    #region Protected Methods

    protected virtual void Awake()
    {
        Stats.Init(this);
        hud.Init(this);
        stats.OnHit += hud.UpdateHUD;
    }

    protected virtual void Update()
    {
        if (IsAlive && stats.CurrentHealth <= 0)
        {
            IsAlive = false;
            OnDeath();
            GameManager.Instance.RaiseOnEnemyDeathEvent(this);
        }
    }

    protected virtual void OnDeath()
    {
        stats.OnDeath();
    }

    #endregion

    #region Public Methods

    public virtual void TakeDamage(int amount)
    {
        stats.Damage(amount);

        GameManager.Instance.Shake(0.1f, 0.1f);
    }

    public void UpdateDurations()
    {
        // Reset Armor
        Stats.ChangeArmor(-1000);
        // Apply all effect
        Stats.Tick();
    }
    public virtual void DrawCards(int value) { }
    public virtual void AddCards(List<CardData> toAdd) { }
    public virtual void GetEnergy(int value) { }

    #endregion
}

