using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isOpen;
    private void OnMouseDown()
    {
        if (!isOpen)
        {
            isOpen = true;
            LootManager.Instance.SetupLoop();
        }
    }
}
