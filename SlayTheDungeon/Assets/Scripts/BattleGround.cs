using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public enum TurnType
{
    PlayerTurn,
    EnemyTurn
}

public enum TurnStatus
{
    Start, 
    Progress, 
    Finish
}

public enum BattleStatus
{
    Peace, 
    Fighting, 
    Finished
}

public class BattleGround : MonoBehaviour
{
    #region Fields

    private Vector3 playerPosition;
    private List<Vector3> enemyPosition;
    [SerializeField] private RectTransform cameraFocus;

    private PlayerData player;

    #endregion

    #region Properties

    public List<Enemy> Enemies { get; private set; } 

    public TurnType TurnType { get; set; }

    public TurnStatus TurnStatus { get; set; }

    public BattleStatus BattleStatus { get; set; }

    #endregion

    #region Protected Methods

    protected void Awake()
    {
        GameManager.Instance.OnCharacterDeath += DeleteCharacter;
        player = GameManager.Instance.Player;

        enemyPosition = new List<Vector3>();
        playerPosition = GameUI.Instance.PlayerPosition.transform.position;
        for (int i = 0; i < GameUI.Instance.EnemyPosition.Count; i++)
        {
            enemyPosition.Add(GameUI.Instance.EnemyPosition[i].position);
        }
    }

    protected void Start()
    {

    }

    #endregion

    #region Public Methods

    public void InitBattle(List<Enemy> enemies)
    {
        Enemies = enemies;
        BattleStatus = BattleStatus.Fighting;

        player.transform.position = new Vector3(playerPosition.x, playerPosition.y, 0);
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            var enemyPosition = this.enemyPosition[i];
            Enemies[i].transform.position = new Vector3(enemyPosition.x, enemyPosition.y, 0) ;
            Enemies[i].gameObject.SetActive(true);
        }
    }

    public void FinishBattle()
    {

    }

    public void IsBattleEnded()
    {
        if (Enemies.Count > 0 && player.IsAlive)
        {
            return;
        }

        GameManager.Instance.TurnEnded = true;
        Enemies.Clear();
        BattleStatus = BattleStatus.Finished;
    }

    public void LeaveBattleGround()
    {
        BattleStatus = BattleStatus.Peace;
    }

    public void DeleteCharacter(CharacterData enemy)
    {
        var a = enemy as Enemy;

        if (!Enemies.Contains(enemy))
            return;

        Enemies.Remove(a);
        Destroy(enemy.gameObject);

        IsBattleEnded();
    }

    #endregion
}
