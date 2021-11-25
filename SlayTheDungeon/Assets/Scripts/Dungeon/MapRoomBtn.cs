using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoomBtn : MonoBehaviour
{
    private GameManager gameManager;
	private Vector2 gridPos;
    private List<MapCorridor> corridors = new List<MapCorridor>();
    public Vector2 GridPos { get => gridPos; set => gridPos = value; }
    public List<MapCorridor> Corridors { get => corridors; set => corridors = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void TryEnterLinkedRoom()
    {
        if (gameManager.CurrentRoom is Corridor)
        {
            return;
        }
        Debug.Log("player currently in " + gameManager.CurrentRoom.GridPos.x + " " + gameManager.CurrentRoom.GridPos.y + "wants to move in " + gridPos.x + " " + gridPos.y);
        float dist = Mathf.Abs(gameManager.CurrentRoom.GridPos.x - gridPos.x) + Mathf.Abs(gameManager.CurrentRoom.GridPos.y - gridPos.y);
        if (dist <= 1 && dist != 0)
        {
            gameManager.MoveToCorridor(gridPos);
        }
    }

    public void RevealAround()
    {
        foreach(var corridor in corridors)
        {
            corridor.gameObject.SetActive(true);
            corridor.MapRoom1.SetActive(true);
            corridor.MapRoom2.SetActive(true);
        }
    }

}
