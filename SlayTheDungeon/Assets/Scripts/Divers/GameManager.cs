using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    [SerializeField] private PlayerData player;

    private DeckPile playerDeck;
    private Hand hand;
    private AudioManager audioManager;

    private GameUI gameUI;
    private bool cameraShaking;

    private DungeonElement currentRoom;
    private CinemachineVirtualCamera currentCamera;
    private float orthographicSizeZoom;
    private MiniMap miniMap;

    private LootManager lootManager;


    #endregion

    #region Properties

    public PlayerData Player => player;

    public bool InBattle { get; private set; }

    public DungeonElement CurrentRoom { get => currentRoom; set => currentRoom = value; }

    public BattleGround BattleGround { get; private set; }

    public bool TurnEnded { get; set; }

    public CinemachineVirtualCamera CurrentCamera => currentCamera;

    public bool PlayerFacingRight { get; set; }

    #endregion

    #region Delegates

    public delegate void OnCharacterDeathEventHandler(CharacterData character);
    public event OnCharacterDeathEventHandler OnCharacterDeath;
    public event EventHandler OnEndTurn;

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();

        BattleGround = GetComponent<BattleGround>();
    }

    protected void Start()
    {
        if (GameUI.HasInstance)
        {
            gameUI = GameUI.Instance;

            hand = gameUI.PlayerHand;
            playerDeck = gameUI.PlayerDeck;
            gameUI.EndTurnButton.onClick.AddListener(EndPlayerTurn);
            gameUI.EndTurnButton.interactable = false;
            miniMap = gameUI.MiniMap;
        }
        lootManager = LootManager.Instance;
        gameUI.StopFight();
        audioManager = AudioManager.Instance;
    }

    #endregion

    #region Public Methods

    public void EnterRoom(DungeonElement room, Transform startPosition)
    {
        if (currentRoom != null)
        {
            currentRoom.gameObject.SetActive(false);
            if (room.GridPos.x > currentRoom.GridPos.x)
            {
                miniMap.MovePlayer(new Vector2(-50f, 0));
            }
            else if (room.GridPos.x < currentRoom.GridPos.x)
            {
                miniMap.MovePlayer(new Vector2(50f, 0));
            }
            else if (room.GridPos.y > currentRoom.GridPos.y)
            {
                miniMap.MovePlayer(new Vector2(0f, -50));
            }
            else if (room.GridPos.y < currentRoom.GridPos.y)
            {
                miniMap.MovePlayer(new Vector2(0, 50));
            }
        }
        if (room is Room)
        {
            gameUI.ShowMap();
            (room as Room).EnterRoom();
            player.Controller.IsMoving = false;
        }
        else
        {
            (room as Corridor).OnEnter();
            gameUI.HideMap();
        }
        room.gameObject.SetActive(true);
        currentRoom = room;
        currentCamera = room.CVCam;
        orthographicSizeZoom = currentCamera.m_Lens.OrthographicSize;
        currentCamera.Follow = Player.transform;
        player.transform.position = new Vector3(startPosition.position.x, startPosition.position.y, 0);
        player.Controller.Flip(true);
    }

    public void MoveToCorridor(Vector2 gridPos)
    {
        Room thisRoom = currentRoom as Room;
        if (currentRoom.GridPos.x - gridPos.x == 1)
        {
            EnterRoom(thisRoom.C_Left, thisRoom.C_Left.EndPoint);
            player.Controller.Flip(false);
        }
        else if (currentRoom.GridPos.x - gridPos.x == -1)
        {
            EnterRoom(thisRoom.C_Right, thisRoom.C_Right.StartPoint);
            player.Controller.Flip(true);
        }
        else if (currentRoom.GridPos.y - gridPos.y == 1)
        {
            EnterRoom(thisRoom.C_Down, thisRoom.C_Down.EndPoint);
            player.Controller.Flip(false);
        }
        else if (currentRoom.GridPos.y - gridPos.y == -1)
        {
            EnterRoom(thisRoom.C_Up, thisRoom.C_Up.StartPoint);
            player.Controller.Flip(true);
        }
        player.Controller.IsMoving = true;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCamera(duration, magnitude));
    }


    public void StartEncounter(List<Enemy> enemies) => StartCoroutine(EncounterEvent(enemies));

    public void RaiseOnEnemyDeathEvent(CharacterData character) => OnCharacterDeath?.Invoke(character);
    public void DrawCards(int amount)
    {
        StartCoroutine(Draw(amount));
    }
    public void AddCards(List<CardData> cards)
    {
        StartCoroutine(AddCardsInHand(cards));
    }

    public void OnPlayerDeath()
    {
        gameUI.EndTurnButton.interactable = false;
        hand.DiscardHand();

        // Change Sounds & Music
        // Check victory or GameOver & say it accordingly
        // Change UI
        gameUI.StopFight();

        GameUI.Instance.GameOverPanel.SetActive(true);
        player.ResetEndCombat();

        InBattle = false;
        BattleGround.FinishBattle();
        // Reset player Status & modifiers
        // Remove combat state & restrictions (movements, inventory, ...)
        BattleGround.LeaveBattleGround();
    }

    public void EndEnnemyTurn()
    {
        Debug.Log(InBattle);
        if (!InBattle) return;
        BattleGround.TurnType = TurnType.PlayerTurn;
        OnEndTurn.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Private Methods

    private void FirstDraw()
    {
        BattleGround.TurnType = TurnType.PlayerTurn;
    }

    private IEnumerator EncounterEvent(List<Enemy> enemies)
    {
        InBattle = true;

        // Announce Fight
        // Change Sounds & Music
        audioManager.PlayCombat();
        // Change UI
        player.Controller.IsMoving = false;
        gameUI.SetupFight(player.Deck);

        BattleGround.InitBattle(enemies);
        yield return new WaitForSeconds(1.0f);
        BattleGround.SpawnEnemies(PlayerFacingRight);
        yield return new WaitForSeconds(0.5f);

        FirstDraw();

        while (BattleGround.BattleStatus != BattleStatus.Finished)
        {
            switch (BattleGround.TurnType)
            {
                case TurnType.PlayerTurn:
                    yield return StartCoroutine(PlayerTurn());
                    break;
                case TurnType.EnemyTurn:
                    yield return StartCoroutine(EnemyTurn());
                    break;
            }
        }

        yield return StartCoroutine(FinishEncounter());
    }

    private IEnumerator FinishEncounter()
    {
        gameUI.EndTurnButton.interactable = false;
        hand.DiscardHand();

        yield return new WaitForSeconds(0.5f);

        // Change Sounds & Music
        audioManager.PlayExplore();
        if (currentRoom is Room) (currentRoom as Room).LinkedDungeonElement.Clean();
        // Check victory or GameOver & say it accordingly
        // Change UI
        gameUI.StopFight();

        yield return new WaitForSeconds(1.0f);

        InBattle = false;
        // Reset player Status & modifiers
        // Provide Loot & exp pts
        // Remove combat state & restrictions (movements, inventory, ...)
        lootManager.SetupLoop(3);
        BattleGround.LeaveBattleGround();
        BattleData.Instance.Reset();
        player.ResetEndCombat();
    }

    private IEnumerator EnemyTurn()
    {
        if (BattleGround.BattleStatus == BattleStatus.Fighting)
        {
            List<Boss> bosses = new List<Boss>();
            foreach (var monster in BattleGround.Enemies)
            {
                monster.UpdateDurations();
            }

            yield return new WaitForSeconds(1.0f);

            foreach (var monster in BattleGround.Enemies)
            {
                if (monster.IsAlive)
                {
                    if (monster is Boss) bosses.Add(monster as Boss);
                    monster.PlayTurn();
                    yield return new WaitForSeconds(1.0f);
                }
            }
            while (bosses.Count > 0)
            {
                yield return new WaitForSeconds(1.0f);
                foreach (Boss boss in bosses)
                {
                    if (!boss.IsPlaying)
                    {
                        boss.DiscardHand();
                        boss.DrawCards(5);
                        bosses.Remove(boss);
                        break;
                    }
                }
            }
        }
        EndEnnemyTurn();
    }

    private IEnumerator PlayerTurn()
    {
        TurnEnded = false;
        player.UpdateDurations();
        player.ResetEnergy();
        BattleGround.TurnType = TurnType.PlayerTurn;

        for (int i = 0; i < 5; i++)
        {
            playerDeck.DrawCard();
            yield return new WaitForSeconds(0.2f);
        }

        foreach (var monster in BattleGround.Enemies)
        {
            if (monster.IsAlive)
            {
                monster.ShowNextAction();
            }
        }

        gameUI.EndTurnButton.interactable = true;

        while (!TurnEnded)
        {
            yield return null;
        }
    }

    private void EndPlayerTurn()
    {
        TurnEnded = true;
        gameUI.EndTurnButton.interactable = false;
        hand.DiscardHand();
        BattleGround.TurnType = TurnType.EnemyTurn;
        OnEndTurn.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        if (cameraShaking) yield break;
        cameraShaking = true;
        var elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            var x = orthographicSizeZoom - Random.Range(0, 2f) * magnitude;

            currentCamera.m_Lens.OrthographicSize = x;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentCamera.m_Lens.OrthographicSize = orthographicSizeZoom;
        cameraShaking = false;
    }
    private IEnumerator Draw(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerDeck.DrawCard();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator AddCardsInHand(List<CardData> toAdd)
    {
        foreach (CardData card in toAdd)
        {
            playerDeck.DrawCard(card);
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion
}

