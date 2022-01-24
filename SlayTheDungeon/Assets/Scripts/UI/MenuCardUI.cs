using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform thisTransform;
    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        thisTransform.SetAsLastSibling();
        thisTransform.DOScale(Vector3.one * 1.2f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisTransform.DOScale(Vector3.one, 0.2f);
    }
}
