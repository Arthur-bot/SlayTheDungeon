using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Corridor : DungeonElement
{
    private List<float> positionTaken = new List<float>();
    [SerializeField] private Transform endPoint;
    [SerializeField] private Door door1;
    [SerializeField] private Door door2;
    // Generation
    [SerializeField] private EnnemyTriger ennemyTriggerPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private List<Transform> hotPoints;
    [SerializeField] private List<GameObject> hotPointsPrefab;
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

    private void Start()
    {
        positionTaken.Add(StartPoint.position.x);
        positionTaken.Add(EndPoint.position.x);
        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        foreach(Transform hotPoint in hotPoints)
        {
            int randomIndex = Random.Range(0, hotPointsPrefab.Count);
            GameObject newElement = Instantiate(hotPointsPrefab[randomIndex], hotPoint.position, Quaternion.identity, transform);
            if (randomIndex == 0)
            {
                newElement.GetComponent<EnnemyTriger>().Level = level;
            }
        }
    }
}
