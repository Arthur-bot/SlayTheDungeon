using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    float moveX;

    private GameManager gameManager;
    private LootManager lootManager;
    private SpriteRenderer sprite;

    public bool facingRight;

    private void Start()
    {
        gameManager = GameManager.Instance;
        lootManager = LootManager.Instance;

        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!gameManager.InBattle && gameManager.CurrentRoom is Corridor && !lootManager.IsLooting)
        {
            moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.position = transform.position + new Vector3(moveX, 0, 0);

            if (moveX > 0f && !facingRight)
            {
                Flip();
                facingRight = true;
            }
            else if (moveX < 0 && facingRight)
            {
                Flip();
                facingRight = false;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        sprite.flipX = !facingRight;

        gameManager.PlayerFacingRight = facingRight;

        gameManager.CurrentCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = facingRight? 0.2f : 0.8f;
    }
}
