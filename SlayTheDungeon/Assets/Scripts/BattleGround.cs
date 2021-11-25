using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [SerializeField] private RectTransform playerPosition;
    [SerializeField] private RectTransform enemyPosition;
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

    }

    #endregion

    #region Public Methods

    public void InitBattle(List<Enemy> enemies)
    {
        Enemies = enemies;
        BattleStatus = BattleStatus.Fighting;

        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);

            //Position enemies
        }
    }

    public void SpawnEnemies()
    {

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
