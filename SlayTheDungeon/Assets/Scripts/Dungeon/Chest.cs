using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerDownHandler
{
    private bool isOpen;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isOpen)
        {
            isOpen = true;
            LootManager.Instance.SetupLoop();
        }
    }

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            isOpen = true;
            LootManager.Instance.SetupLoop();
        }
    }
}
