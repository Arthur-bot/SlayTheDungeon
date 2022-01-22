using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MiniMap : MonoBehaviour
{
    int gridSizeX, gridSizeY;
    [SerializeField] private MapCorridor corridorMapPrefab;
    [SerializeField] private MapRoomBtn roomMapPrefab;
    [SerializeField] private Transform mapRoot;
    [SerializeField] private Transform corridorRoot;
    [SerializeField] private RectTransform playerIcon;
    private RectTransform thisTransform;
    private int zoomLevel = 3;

    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
    }

    public MapRoomBtn AddRoom(Vector2 pos)
    {
        MapRoomBtn newBtn = Instantiate(roomMapPrefab, mapRoot);
        newBtn.GridPos = pos + new Vector2(gridSizeX, gridSizeY);
        newBtn.GetComponent<RectTransform>().anchoredPosition = pos * 100;
        newBtn.gameObject.SetActive(false);
        return newBtn;
    }

    public MapCorridor AddCorridor(Vector2 pos, float angle, MapRoomBtn room1, MapRoomBtn room2)
    {
        MapCorridor newMapCorridor = Instantiate(corridorMapPrefab, Vector3.zero, Quaternion.Euler(0, 0, angle), corridorRoot);
        newMapCorridor.GetComponent<RectTransform>().anchoredPosition = (pos  - new Vector2(gridSizeX, gridSizeY)) * 100;
        newMapCorridor.MapRoom1 = room1.gameObject;
        newMapCorridor.MapRoom2 = room2.gameObject;
        newMapCorridor.gameObject.SetActive(false);
        return newMapCorridor;
    }

    public void SetWorldSize(int x, int y)
    {
        gridSizeX = x;
        gridSizeY = y;
    }

    public void MovePlayer(Vector2 direction)
    {
        playerIcon.anchoredPosition -= direction;
        thisTransform.anchoredPosition += direction *transform.localScale.x;
    }
    public void Recenter()
    {
        thisTransform.DOAnchorPos(playerIcon.anchoredPosition * -1, 1.0f);
        thisTransform.DOScale(Vector3.one, 1.0f).OnComplete(() => {
            zoomLevel = 3;
        });

    }
    public void ZoomIn()
    {
        if (zoomLevel > 0)
        {
            thisTransform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f, 1f);
            zoomLevel--;
        }
    }
    public void ZoomOut()
    {
        if (zoomLevel < 6)
        {
            thisTransform.localScale = new Vector3(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f, 1f);
            zoomLevel++;
        }
    }
}
