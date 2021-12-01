using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardLoot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CardUI cardUI;
    private bool isSelected;
    private RectTransform thisTransform;
    private LootManager lootManager;

    public RectTransform ThisTransform { get => thisTransform; set => thisTransform = value; }

    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
        cardUI = GetComponent<CardUI>();
    }
    private void Start()
    {
        lootManager = LootManager.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSelected)
        {
            lootManager.PickLoot(this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
        cardUI.Highlight(Color.white);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
        cardUI.DisableHighlight();
    }
}
