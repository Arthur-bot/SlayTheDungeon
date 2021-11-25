using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : DungeonElement
{
    private Corridor? c_Left = null;
    private Corridor? c_Right = null;
    private Corridor? c_Up = null;
    private Corridor? c_Down = null;
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
}
