using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : CharacterData
{
    private float initialX;
    private int attackIndex;

    public EnnemyData EnnemyData { get; set; }
    [SerializeField] private RectTransform preshotTransform;

    public virtual void SetupEnemy()
    {
        stats.BaseStats.Copy(EnnemyData.Stats.BaseStats);
        hud.CharacterSprite.sprite = EnnemyData.Sprite;
        float size = Mathf.Max(EnnemyData.Sprite.bounds.size.x, EnnemyData.Sprite.bounds.size.y);
        hud.CharacterSprite.GetComponent<RectTransform>().localScale = new Vector2(size * (GameManager.Instance.PlayerFacingRight ? 1 : -1), size);
        if (preshotTransform != null) preshotTransform.localScale = new Vector2( 1 /size * (GameManager.Instance.PlayerFacingRight ? 1 : -1), 1 / size);
    }

    public virtual void PlayTurn()
    {
        TargetingSystem.Instance.SetTarget(this);
        initialX = transform.localPosition.x;
        var destination = transform.localPosition.x + (GameManager.Instance.PlayerFacingRight? -1 : 1);

        transform.DOLocalMoveX(destination, 0.15f).OnComplete(ResetPosition);

        var attack = EnnemyData.Attacks[attackIndex];
        BattleData.Instance.CurrentTarget = attack.TargetType;
        switch (attack.TargetType)
        {
            case Target.Aoe:
                attack.ApplyEffect(this, GameManager.Instance.BattleGround.Enemies);
                break;
            case Target.Self:
                attack.ApplyEffect(this, TargetingSystem.Instance.getTarget());
                break;
            case Target.SingleTarget:
                attack.ApplyEffect(this, GameManager.Instance.Player);
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
        if(EnnemyData.Boss) return;

        attackIndex += 1;
        if (attackIndex >= EnnemyData.Attacks.Count)
        {
            attackIndex = 0;
        }

        var attack = EnnemyData.Attacks[attackIndex];
        hud.NextAction(attack);
    }
}
