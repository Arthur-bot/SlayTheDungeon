using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRoomBtn : MonoBehaviour
{
    [SerializeField] private Image icon;
    private GameManager gameManager;
    private FirecampManager firecampManager;
    private LootManager lootManager;
	private Vector2 gridPos;
    private List<MapCorridor> corridors = new List<MapCorridor>();
    public Vector2 GridPos { get => gridPos; set => gridPos = value; }
    public List<MapCorridor> Corridors { get => corridors; set => corridors = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        firecampManager = FirecampManager.Instance;
        lootManager = LootManager.Instance;
    }
    public void TryEnterLinkedRoom()
    {
        if (gameManager.CurrentRoom is Corridor || firecampManager.IsOpen || lootManager.IsLooting)
        {
            return;
        }
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
    public void SetupIcone(Sprite sprite)
    {
        if (sprite != null)
            icon.sprite = sprite;
        else
            icon.gameObject.SetActive(false);
    }

}
