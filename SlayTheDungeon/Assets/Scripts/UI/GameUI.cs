using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : Singleton<GameUI>
{
    #region Fields

    [SerializeField] private Hand playerHand;
    [SerializeField] private HandAnimation handAnimation;
    [SerializeField] private DeckPile playerDeck;
    [SerializeField] private Button endTurnButton;

    [SerializeField] private GameObject fightMaterial;
    [SerializeField] private Vector2 positionFightUI;

    #endregion

    #region Properties

    public Hand PlayerHand => playerHand;

    public HandAnimation HandAnimation => handAnimation;

    public DeckPile PlayerDeck => playerDeck;

    public Button EndTurnButton => endTurnButton;

    #endregion

    #region Public Methods
    public void SetupFight()
    {
        fightMaterial.transform.DOLocalMoveY(positionFightUI.x, 0.5f).OnStart(EnableFight);
    }

    public void StopFight()
    {
        fightMaterial.transform.DOLocalMoveY(positionFightUI.y, 0.5f).OnComplete(DisableFight);
    }

    public void EnableFight()
    {
        fightMaterial.SetActive(true);
    }
    public void DisableFight()
    {
        fightMaterial.SetActive(false);
    }

    #endregion
}
