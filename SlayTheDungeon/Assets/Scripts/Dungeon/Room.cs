using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : DungeonElement
{

    private Corridor? c_Left;
    private Corridor? c_Right;
    private Corridor? c_Up;
    private Corridor? c_Down;

    public Corridor C_Left { get => c_Left; set => c_Left = value; }
    public Corridor C_Right { get => c_Right; set => c_Right = value; }
    public Corridor C_Up { get => c_Up; set => c_Up = value; }
    public Corridor C_Down { get => c_Down; set => c_Down = value; }
}
