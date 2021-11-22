using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    float moveX;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void Update()
    {
        if (!gameManager.InFight)
        {
            moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.Translate(new Vector3(moveX, 0, 0));
        }
    }
}
