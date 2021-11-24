using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : DungeonElement
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private Door door1;
    [SerializeField] private Door door2;
    private Room room1;
    private Room room2;

    public Room Room1 { get => room1; set => room1 = value; }
    public Room Room2 { get => room2; set => room2 = value; }
    public Transform EndPoint { get => endPoint; set => endPoint = value; }
    public Door Door1 { get => door1; set => door1 = value; }
    public Door Door2 { get => door2; set => door2 = value; }

    public void SetRooms(Room _room1, Room _room2)
    {
        room1 = _room1;
        room2 = _room2;
        door1.SetRoom(_room1);
        door2.SetRoom(_room2);
    }
}
