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
    [SerializeField] private GameObject ennemyTriggerPrefab;
    [SerializeField] private GameObject boxPrefab;
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
        Instantiate(ennemyTriggerPrefab, new Vector3(newPosition(), 0, 0), Quaternion.identity, transform);
        Instantiate(boxPrefab, new Vector3(newPosition(), -3.5f, 0), Quaternion.identity, transform);
        float randomAddEnnemy = Random.Range(0, 100);
        if (randomAddEnnemy > 60)
        {
            Instantiate(ennemyTriggerPrefab, new Vector3(newPosition(), 0, 0), Quaternion.identity, transform);
        }
        float randomAddBox = Random.Range(0, 100);
        if (randomAddEnnemy > 80)
        {
            Instantiate(boxPrefab, new Vector3(newPosition(), -3.5f, 0), Quaternion.identity, transform);
        }
    }

    private float newPosition()
    {
        float newX = Random.Range(StartPoint.position.x, EndPoint.position.x);
        while(!PositionIsFree(newX))
        {
            newX = Random.Range(StartPoint.position.x, EndPoint.position.x);
        }
        return newX;
    }
    private bool PositionIsFree(float x)
    {
        foreach(float position in positionTaken)
        {
            if (Mathf.Abs(x - position) < 4)
            {
                return false;
            }
        }
        return true;
    }
}
