using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    None,
    Boss,
    Monster,
    Firecamp,
    Chest
}
public class Room : DungeonElement
{
    [SerializeField] private List<GameObject> roomComponents;
    private Corridor c_Left;
    private Corridor c_Right;
    private Corridor c_Up;
    private Corridor c_Down;
    private MapRoomBtn btn;


    public Corridor C_Left { get => c_Left; set => c_Left = value; }
    public Corridor C_Right { get => c_Right; set => c_Right = value; }
    public Corridor C_Up { get => c_Up; set => c_Up = value; }
    public Corridor C_Down { get => c_Down; set => c_Down = value; }
    public MapRoomBtn Btn { get => btn; set => btn = value; }

    public void EnterRoom()
    {
        btn.gameObject.SetActive(true);
        btn.RevealAround();
    }

    public void SetupRoom(RoomType type)
    {
        foreach(GameObject component in roomComponents)
        {
            component.SetActive(false);
        }
        roomComponents[(int)RoomType.Monster - 1].GetComponent<EnnemyTriger>().Level = level;
        if (type != RoomType.None)
        {
            roomComponents[(int)type - 1].SetActive(true);
        }
    }
}
