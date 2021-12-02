using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : CharacterData
{
    [SerializeField] private int damageValue;

    private float initialX;

    public void Attack()
    {
        initialX = transform.localPosition.x;
        var destination = transform.localPosition.x + (GameManager.Instance.PlayerFacingRight? -1 : 1);

        transform.DOLocalMoveX(destination, 0.15f).OnComplete(ResetPosition);
        GameManager.Instance.Player.TakeDamage(damageValue);
    }

    public void ResetPosition()
    {
        transform.DOLocalMoveX(initialX, 0.3f);
    }
}
