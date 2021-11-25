using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTriger : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<Enemy> enemies;

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
            gameManager.StartEncounter(enemies);
            Destroy(this);
        }
    }

    #endregion
}
