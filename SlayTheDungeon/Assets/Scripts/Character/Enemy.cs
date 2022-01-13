using System.Collections;
using DG.Tweening;
using UnityEngine.Rendering;

public class Enemy : CharacterData
{
    private float initialX;
    private int attackIndex;

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

        var attack = EnnemyData.Attacks[attackIndex];
        switch (attack.TargetType)
        {
            case Target.Aoe:
                attack.ApplyEffect(GameManager.Instance.BattleGround.Enemies);
                break;
            case Target.Self:
                attack.ApplyEffect(TargetingSystem.Instance.getTarget());
                break;
            case Target.SingleTarget:
                attack.ApplyEffect(GameManager.Instance.Player);
                break;
        }

        hud.HideAction();
    }

    public void ResetPosition()
    {
        transform.DOLocalMoveX(initialX, 0.3f);
    }

    public void ShowNextAction()
    {
        attackIndex += 1;
        if (attackIndex >= EnnemyData.Attacks.Count)
        {
            attackIndex = 0;
        }

        var attack = EnnemyData.Attacks[attackIndex];
        hud.NextAction(attack);
    }
}
