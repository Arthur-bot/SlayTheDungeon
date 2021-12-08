using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTriger : MonoBehaviour
{
    #region Fields

    [SerializeField] private Enemy enemyPrefab;
    private List<Enemy> enemies = new List<Enemy>();
    private GameManager gameManager;

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
            Enemy newEnemy = Instantiate(enemyPrefab, transform);
            newEnemy.EnnemyData = DataBase.Instance.PickRandomEnnemy();
            newEnemy.SetupEnemy();
            enemies.Add(newEnemy);
            gameManager.StartEncounter(enemies);
            Destroy(this);
        }
    }

    #endregion
}
