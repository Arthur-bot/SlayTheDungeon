using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    float moveX;
    private GameManager gameManager;
    private LootManager lootManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        lootManager = LootManager.Instance;
    }
    private void Update()
    {
        if (!gameManager.InBattle && gameManager.CurrentRoom is Corridor && !lootManager.IsLooting)
        {
            moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.position = transform.position + new Vector3(moveX, 0, 0);
        }
    }
}
