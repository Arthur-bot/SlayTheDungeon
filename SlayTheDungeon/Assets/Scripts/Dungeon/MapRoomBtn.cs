using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoomBtn : MonoBehaviour
{
    private GameManager gameManager;
	private Vector2 gridPos;
    public Vector2 GridPos { get => gridPos; set => gridPos = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void TryEnterLinkedRoom()
    {
        gameManager.MoveToCorridor(gridPos);
    }

}
