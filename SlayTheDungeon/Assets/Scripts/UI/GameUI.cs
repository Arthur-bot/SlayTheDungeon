using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : Singleton<GameUI>
{
    #region Fields

    [SerializeField] private Hand playerHand;
    [SerializeField] private CardAnimation handAnimation;
    [SerializeField] private DamageUI damageUI;
    [SerializeField] private DeckPile playerDeck;
    [SerializeField] private DiscardPile discardPile;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private MiniMap miniMap;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private List<Transform> rightPositions;
    [SerializeField] private List<Transform> leftPositions;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject fightMaterial;
    [SerializeField] private GameObject exploreMaterial;
    [SerializeField] private Vector2 positionFightUI;
    [SerializeField] private Vector2 positionExploreUI;

    #endregion

    #region Properties

    public Hand PlayerHand => playerHand;

    public CardAnimation HandAnimation => handAnimation;

    public DamageUI DamageUI => damageUI;

    public DeckPile PlayerDeck => playerDeck;

    public DiscardPile DiscardPile => discardPile;

    public Button EndTurnButton => endTurnButton;

    public MiniMap MiniMap => miniMap;

    public Transform PlayerPosition => playerPosition;

    public List<Transform> RightPositions => rightPositions;

    public List<Transform> LeftPositions => leftPositions;

    public TextMeshProUGUI EnergyText => energyText;

    public GameObject GameOverPanel => gameOverPanel;

    #endregion

    #region Public Methods
    public void SetupFight(List<CardData> cards)
    {
        discardPile.ResetDiscardPile();
        playerDeck.ResetDeckPile(cards);

        exploreMaterial.transform.DOLocalMoveY(positionFightUI.y, 0.5f).OnComplete(() => {
            EnableFight();
            fightMaterial.transform.DOLocalMoveY(positionFightUI.x, 0.5f);
        });
    }

    public void StopFight()
    {
        fightMaterial.transform.DOLocalMoveY(positionFightUI.y, 0.5f).OnComplete(() => {
            DisableFight();
            exploreMaterial.transform.DOLocalMoveY(positionFightUI.x, 0.5f);
        });
    }

    public void EnableFight()
    {
        fightMaterial.SetActive(true);
        exploreMaterial.SetActive(false);
    }

    public void DisableFight()
    {
        fightMaterial.SetActive(false);
        exploreMaterial.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#endif

        Application.Quit();
    }

#endregion
}
