using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterData
{
    [SerializeField] private int damageValue;

    public void Attack()
    {
        GameManager.Instance.Player.TakeDamage(damageValue);
    }
}
