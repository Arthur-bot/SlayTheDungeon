using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTriger : MonoBehaviour
{
    #region Fields

    [SerializeField] private Enemy enemyPrefab;
    private List<Enemy> enemies = new List<Enemy>();
    private GameManager gameManager;
    private int level;

    public int Level { get => level; set => level = value; }

    #endregion

    #region Protected Methods

    protected void Start()
    {
        gameManager = GameManager.Instance;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            List<EnnemyData> combination = DataBase.Instance.PickRandomEnnemyCombination(level);
            foreach(EnnemyData enemy in combination)
            {
                Enemy newEnemy = Instantiate(enemyPrefab, transform);
                newEnemy.EnnemyData = enemy;
                newEnemy.SetupEnemy();
                enemies.Add(newEnemy);
            }
            gameManager.StartEncounter(enemies);
            Destroy(this);
        }
    }

    #endregion
}
