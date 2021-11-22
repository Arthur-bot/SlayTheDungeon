using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : MonoBehaviour
{
    #region Fields

    [SerializeField] private int maxHealth;

    private int health;
    [SerializeField] private int armor;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Slider healthSlider;

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        health = maxHealth;
        healthText.text = health + "/" + maxHealth;

        armorText.text = armor.ToString();
        armorText.gameObject.SetActive(armor != 0);
    }

    #endregion

    public void TakeDamage(int amount)
    {
        GameUI.Instance.DamageUI.NewDamage(amount, transform.position);
        int lifeDamage = amount - armor;
        armorText.gameObject.SetActive(armor != 0);
        armor = Mathf.Max(armor - amount, 0);
        if (lifeDamage > 0)
        {
            health -= lifeDamage;
            StartCoroutine(GameManager.Instance.ShakeCamera(0.1f, 0.1f));
            healthSlider.value = health / (float)maxHealth;
        }
        healthText.text = health + "/" + maxHealth;
        armorText.text = armor.ToString();
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
        healthText.text = health.ToString();
    }

    public void StackArmor(int amount)
    {
        armor += amount;
        armorText.text = armor.ToString();
        armorText.gameObject.SetActive(armor != 0);
    }
}

