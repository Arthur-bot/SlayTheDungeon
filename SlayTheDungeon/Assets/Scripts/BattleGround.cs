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
    private List<Transform> enemyLeftPosition;
    private List<Transform> enemyRightPosition;
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

        enemyLeftPosition = new List<Transform>();
        enemyRightPosition = new List<Transform>();
    }

    protected void Start()
    {
        foreach (var position in GameUI.Instance.LeftPositions)
        {
            enemyLeftPosition.Add(position);
        }

        foreach (var position in GameUI.Instance.RightPositions)
        {
            enemyRightPosition.Add(position);
        }
    }

    #endregion

    #region Public Methods

    public void InitBattle(List<Enemy> enemies)
    {
        Enemies = enemies;
        BattleStatus = BattleStatus.Fighting;
    }

    public void SpawnEnemies(bool facingRight)
    {
        if (facingRight)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                var enemyPosition = enemyRightPosition[i];
                Enemies[i].transform.position = new Vector3(enemyPosition.position.x, enemyPosition.position.y, 0);
                Enemies[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                var enemyPosition = enemyLeftPosition[i];
                Enemies[i].transform.position = new Vector3(enemyPosition.position.x, enemyPosition.position.y, 0);
                Enemies[i].gameObject.SetActive(true);
            }
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
