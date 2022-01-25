using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTriger : MonoBehaviour
{
    #region Fields

    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Boss bossPrefab;
    private List<Enemy> enemies = new List<Enemy>();
    private GameManager gameManager;
    private int level;
    private bool bossTrigger;
    private bool alreadyUsed;

    public int Level { get => level; set => level = value; }
    public bool BossTrigger { get => bossTrigger; set => bossTrigger = value; }
    public bool AlreadyUsed { get => alreadyUsed; set => alreadyUsed = value; }

    #endregion

    #region Protected Methods

    protected void Start()
    {
        gameManager = GameManager.Instance;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyUsed) return;
        if (collision.GetComponent<PlayerController>())
        {
            List<EnnemyData> combination;
            if (bossTrigger)
            {
                combination = DataBase.Instance.PickRandomBoss();
            }
            else
            {
                combination = DataBase.Instance.PickRandomEnnemyCombination(level);
            }
            foreach(EnnemyData enemy in combination)
            {
                Enemy newEnemy;
                if (enemy.Boss)
                    newEnemy = Instantiate(bossPrefab, transform);
                else
                    newEnemy = Instantiate(enemyPrefab, transform);
                newEnemy.EnnemyData = enemy;
                newEnemy.SetupEnemy();
                enemies.Add(newEnemy);
            }
            gameManager.StartEncounter(enemies);
            alreadyUsed = true;
        }
    }

    #endregion
}
