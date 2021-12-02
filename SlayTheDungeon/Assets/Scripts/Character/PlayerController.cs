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

    private bool facingRight { get; set; } = true;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
    }
    private void Start()
    {
        lootManager = LootManager.Instance;
    }

    private void Update()
    {
        if (!gameManager.InBattle && gameManager.CurrentRoom is Corridor && !lootManager.IsLooting)
        {
            moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.position = transform.position + new Vector3(moveX, 0, 0);

            if (moveX > 0f && !facingRight)
            {
                Flip(true);
                facingRight = true;
            }
            else if (moveX < 0 && facingRight)
            {
                Flip(false);
                facingRight = false;
            }
        }
    }

    public void Flip(bool isFacingRight)
    {
        if(isFacingRight)
        {
            facingRight = true;
            sprite.flipX = false;

            gameManager.PlayerFacingRight = true;

            gameManager.CurrentCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.2f;
        }
        else
        {
            facingRight = false;
            sprite.flipX = true;

            gameManager.PlayerFacingRight = false;

            gameManager.CurrentCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.8f;
        }


        //facingRight = !facingRight;
        //sprite.flipX = !facingRight;

        //gameManager.PlayerFacingRight = facingRight;

        //gameManager.CurrentCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = facingRight? 0.2f : 0.8f;
    }
}
