using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DungeonElement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private CinemachineVirtualCamera cvcam;
    protected int level;

    protected GameManager gameManager;
    protected Vector2 gridPos;
    public Vector2 GridPos { get => gridPos; set => gridPos = value; }
    public Transform StartPoint { get => startPoint; set => startPoint = value; }
    public CinemachineVirtualCamera CVCam => cvcam;

    public int Level { get => level; set => level = value; }

    void Awake()
    {
        gameManager = GameManager.Instance;
    }
    // Start is called before the first frame update
    public virtual void OnEnter()
    {

    }
}
