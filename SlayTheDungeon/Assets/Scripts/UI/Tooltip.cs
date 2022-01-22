using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform bgRectTransform;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private RectTransform currentParent;
    public void ShowTooltip(string tooltipMessage)
    {
        tooltip.SetActive(true);
        text.text = tooltipMessage;
        Vector2 bgSize = new Vector2(text.preferredWidth + 20, text.preferredHeight + 20);
        bgRectTransform.sizeDelta = bgSize;
        bgRectTransform.SetParent(currentParent);
        bgRectTransform.anchoredPosition = Vector2.down * 100;
    }

    public void SetParent(RectTransform parent)
    {
        parent.SetAsLastSibling();
        currentParent = parent;
    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
