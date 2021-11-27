using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour
{
    #region Fields

    [Header("Stats")]
    [SerializeField] protected StatSystem stats;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Slider healthSlider;

    #endregion

    #region Properties

    public StatSystem Stats => stats;

    public bool IsAlive { get; private set; } = true;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        Stats.Init(this);
        UpdateHUD(this);

        stats.OnHit += UpdateHUD;
    }

    protected void Update()
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

    public void TakeDamage(int amount)
    {
        stats.Damage(amount);

        StartCoroutine(GameManager.Instance.ShakeCamera(0.1f, 0.1f));
    }

    public void UpdateHUD(CharacterData data)
    {
        healthText.text = stats.CurrentHealth + "/" + stats.StatsCopy.Health;
        armorText.text = stats.CurrentArmor.ToString();
        armorText.gameObject.SetActive(stats.CurrentArmor > 0);
        healthSlider.value = stats.CurrentHealth / (float)stats.StatsCopy.Health;
    }

    public void UpdateDurations()
    {
        Stats.Tick();
    }

    #endregion
}

