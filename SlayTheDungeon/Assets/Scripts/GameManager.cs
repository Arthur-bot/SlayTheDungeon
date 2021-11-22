using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    [SerializeField] private CharacterData player;
    [SerializeField] private List<Enemy> enemies;

    private DeckPile playerDeck;
    private bool isPlayerTurn;
    private Hand hand;
    private GameUI gameUI;
    private bool inFight;
    private Camera currentCamera;
    private bool cameraShaking;

    #endregion

    #region Properties

    public bool IsPlayerTurn { get => isPlayerTurn; set => isPlayerTurn = value; }

    public List<Enemy> Enemies { get => enemies; set => enemies = value; }

    public CharacterData Player => player;

    public bool InFight => inFight;

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();

        currentCamera = Camera.main;
    }

    protected void Start()
    {
        if (GameUI.HasInstance)
        {
            gameUI = GameUI.Instance;

            hand = gameUI.PlayerHand;
            playerDeck = gameUI.PlayerDeck;
            gameUI.EndTurnButton.onClick.AddListener(EndTurn);
        }
        gameUI.StopFight();
    }

    #endregion

    #region Public Methods
    public void EndTurn()
    {
        if (isPlayerTurn) // End of player's turn
        {
            isPlayerTurn = false;
            hand.DiscardHand();
            StartCoroutine(EnnemyTurn());
        }
        else // Start of player's turn
        {
            StartCoroutine(DrawHand());
            isPlayerTurn = true;
        }
    }
    public void StartCombat()
    {
        inFight = true;
        gameUI.SetupFight();
        // Start combat 
        isPlayerTurn = false;
        Invoke("EndTurn",1.0f);
    }

    public void EndCombat()
    {
        gameUI.StopFight();
        inFight = false;
    }

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        if (cameraShaking) yield break;

        cameraShaking = true;
        var elapsedTime = 0.0f;
        var initialPosition = currentCamera.gameObject.transform.position;
        var zoom = currentCamera.orthographicSize;

        while (elapsedTime < duration)
        {
            var x = zoom - Random.Range(0, 2f) * magnitude;

            currentCamera.orthographicSize = x;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentCamera.orthographicSize = zoom;
        currentCamera.gameObject.transform.position = initialPosition;
        cameraShaking = false;
    }

    #endregion

    #region Private Methods

    private IEnumerator EnnemyTurn()
    {
        foreach (Enemy monster in enemies)
        {
            monster.Attack();
            yield return new WaitForSeconds(1.0f);
        }

        EndTurn();
    }
    private IEnumerator DrawHand()
    {
        for (int i = 0; i < 5; i++)
        {
            playerDeck.DrawCard();
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion
}
