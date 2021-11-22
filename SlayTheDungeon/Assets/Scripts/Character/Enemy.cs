using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : CharacterData
{
    [SerializeField] private int damageValue;

    public void Attack()
    {
        transform.DOLocalMoveX(transform.localPosition.x -1f, 0.15f).OnComplete(ResetPosition);
        GameManager.Instance.Player.TakeDamage(damageValue);
    }

    public void ResetPosition()
    {
        transform.DOLocalMoveX(transform.localPosition.x + 1f, 0.3f);
    }
}
