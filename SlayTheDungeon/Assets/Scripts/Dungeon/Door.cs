using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Room room;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.EnterRoom(room, room.StartPoint);
        }
    }
    public void SetRoom(Room _room)
    {
        room = _room;
    }
}
