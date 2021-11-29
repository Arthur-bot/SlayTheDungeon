using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapScroller : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform mapTransform;
    [SerializeField] private int sensitivity = 200;
    private Vector2 previousPos;
    private Vector3 previousMousePos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        previousMousePos = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mouseMove = Input.mousePosition - previousMousePos;
        previousMousePos = Input.mousePosition;
        mapTransform.anchoredPosition += mouseMove * sensitivity * Time.deltaTime;
    }
}
