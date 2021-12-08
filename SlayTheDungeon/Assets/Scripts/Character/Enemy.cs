using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : CharacterData
{
    private EnnemyData ennemyData;
    [SerializeField] private Image image;

    private float initialX;
    public EnnemyData EnnemyData { get => ennemyData; set => ennemyData = value; }

    public void SetupEnemy()
    {
        stats.BaseStats.Copy(ennemyData.Stats.BaseStats);
        image.sprite = ennemyData.Sprite;
    }

    public void Attack()
    {
        TargetingSystem.Instance.SetTarget(this);
        initialX = transform.localPosition.x;
        var destination = transform.localPosition.x + (GameManager.Instance.PlayerFacingRight? -1 : 1);

        transform.DOLocalMoveX(destination, 0.15f).OnComplete(ResetPosition);
        ennemyData.Attack();
    }

    public void ResetPosition()
    {
        transform.DOLocalMoveX(initialX, 0.3f);
    }
}
