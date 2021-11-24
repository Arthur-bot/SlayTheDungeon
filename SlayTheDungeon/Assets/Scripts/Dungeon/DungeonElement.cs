using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonElement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    protected GameManager gameManager;
    protected Vector2 gridPos;
    public Vector2 GridPos { get => gridPos; set => gridPos = value; }
    public Transform StartPoint { get => startPoint; set => startPoint = value; }
    void Awake()
    {
        gameManager = GameManager.Instance;
    }
    // Start is called before the first frame update
}
