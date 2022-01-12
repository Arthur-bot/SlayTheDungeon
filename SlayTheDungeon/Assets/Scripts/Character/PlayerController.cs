using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private RectTransform sprite;
    private bool isMoving;
    private GameManager gameManager;


    private bool facingRight { get; set; } = true;
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (isMoving)
        {
            float moveX = (facingRight? 1 : -1) * speed * Time.deltaTime;
            transform.position = transform.position + new Vector3(moveX, 0, 0);
        }
    }

    public void Flip(bool isFacingRight)
    {
        if(isFacingRight)
        {
            facingRight = true;
            sprite.localScale = Vector3.one;

            gameManager.PlayerFacingRight = true;

            gameManager.CurrentCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.2f;
        }
        else
        {
            facingRight = false;
            sprite.localScale = new Vector3(-1, 1, 1);

            gameManager.PlayerFacingRight = false;

            gameManager.CurrentCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.8f;
        }
    }
}
