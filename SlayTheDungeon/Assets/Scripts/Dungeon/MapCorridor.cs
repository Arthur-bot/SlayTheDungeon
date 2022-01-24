using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCorridor : MonoBehaviour
{
    private GameObject mapRoom1;
    private GameObject mapRoom2;
    [SerializeField] private Image background;

    public GameObject MapRoom1 { get => mapRoom1; set => mapRoom1 = value; }
    public GameObject MapRoom2 { get => mapRoom2; set => mapRoom2 = value; }

    public void Enter()
    {
        background.color = Color.gray;
    }
}
