using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    #region Fields

    [SerializeField] private PlayerData player;

    private DeckPile playerDeck;
    private Hand hand;

    private GameUI gameUI;
    private Camera currentCamera;
    private bool cameraShaking;

    private DungeonElement currentRoom;
    private MiniMap miniMap;

    private LootManager lootManager;


    #endregion

    #region Properties

    public PlayerData Player => player;

    public bool InBattle { get; private set; }

    public DungeonElement CurrentRoom { get => currentRoom; set => currentRoom = value; }

    public BattleGround BattleGround { get; private set; }

    public bool TurnEnded { get; set; }

    #endregion

    #region Delegates

    public delegate void OnCharacterDeathEventHandler(CharacterData character);
    public event OnCharacterDeathEventHandler OnCharacterDeath;

    #endregion

    #region Protected Methods

    protected override void OnAwake()
    {
        base.OnAwake();

        currentCamera = Camera.main;
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
            (room as Room).EnterRoom();
        }
        room.gameObject.SetActive(true);
        currentRoom = room;
        player.transform.position = new Vector3(startPosition.position.x, startPosition.position.y, 0);
    }

    public void MoveToCorridor(Vector2 gridPos)
    {
        Room thisRoom = currentRoom as Room;
        if (currentRoom.GridPos.x - gridPos.x == 1)
        {
            EnterRoom(thisRoom.C_Left, thisRoom.C_Left.EndPoint);
        }
        else if (currentRoom.GridPos.x - gridPos.x == -1)
        {
            EnterRoom(thisRoom.C_Right, thisRoom.C_Right.StartPoint);
        }
        else if (currentRoom.GridPos.y - gridPos.y == 1)
        {
            EnterRoom(thisRoom.C_Down, thisRoom.C_Down.EndPoint);
        }
        else if (currentRoom.GridPos.y - gridPos.y == -1)
        {
            EnterRoom(thisRoom.C_Up, thisRoom.C_Up.StartPoint);
        }
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

    public void StartEncounter(List<Enemy> enemies) => StartCoroutine(EncounterEvent(enemies));

    public void RaiseOnEnemyDeathEvent(CharacterData character) => OnCharacterDeath?.Invoke(character);

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
        // Change UI
        gameUI.SetupFight(player.Deck);

        BattleGround.InitBattle(enemies);
        yield return new WaitForSeconds(1.0f);
        BattleGround.SpawnEnemies();
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
        // Check victory or GameOver & say it accordingly
        // Change UI
        gameUI.StopFight();

        yield return new WaitForSeconds(1.0f);

        InBattle = false;
        BattleGround.FinishBattle();
        // Reset player Status & modifiers
        // Provide Loot & exp pts
        // Remove combat state & restrictions (movements, inventory, ...)
        lootManager.SetupLoop();
        BattleGround.LeaveBattleGround();
    }

    private IEnumerator EnemyTurn()
    {
        foreach (var monster in BattleGround.Enemies)
        {
            monster.UpdateDurations();
        }

        yield return new WaitForSeconds(1.0f);

        foreach (var monster in BattleGround.Enemies)
        {
            if (monster.IsAlive)
            {
                monster.Attack();
                yield return new WaitForSeconds(1.0f);
            }
        }

        BattleGround.TurnType = TurnType.PlayerTurn;
        yield return new WaitForEndOfFrame();
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
    }

    #endregion
}

