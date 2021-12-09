using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : CharacterData
{
    private float initialX;

    public EnnemyData EnnemyData { get; set; }

    public void SetupEnemy()
    {
        stats.BaseStats.Copy(EnnemyData.Stats.BaseStats);
        hud.CharacterSprite.sprite = EnnemyData.Sprite;
    }

    public void Attack()
    {
        TargetingSystem.Instance.SetTarget(this);
        initialX = transform.localPosition.x;
        var destination = transform.localPosition.x + (GameManager.Instance.PlayerFacingRight? -1 : 1);

        transform.DOLocalMoveX(destination, 0.15f).OnComplete(ResetPosition);
        EnnemyData.Attack();
    }

    public void ResetPosition()
    {
        transform.DOLocalMoveX(initialX, 0.3f);
    }
}
