using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    #region Fields

    [SerializeField] private int maxHealth;

    private int health;
    [SerializeField] private int armor;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    // Start is called before the first frame update

    #endregion

    private void Start()
    {
        health = maxHealth;
        healthText.text = "Health : " + health;
        armorText.text = "Armor : " + armor;
    }

    public void TakeDamage(int amount)
    {
        int lifeDamage = amount - armor;
        armor = Mathf.Max(armor - amount, 0);
        if (lifeDamage > 0)
        {
            health -= lifeDamage;
        }
        healthText.text = "Health : " + health;
        armorText.text = "Armor : " + armor;
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(maxHealth, health + amount);
        healthText.text = health.ToString();
    }

    public void StackArmor(int amount)
    {
        armor += amount;
        armorText.text = health.ToString();
    }
}

