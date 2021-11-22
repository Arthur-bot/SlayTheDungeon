using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyTriger : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private List<Enemy> enemies;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
            }
            gameManager.Enemies = enemies;
            gameManager.StartCombat();
        }
    }
}
