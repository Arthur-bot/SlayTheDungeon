using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScroll : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float limit;
    [SerializeField] private float speed;
    private float initY;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initY = rectTransform.offsetMax.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTop();
        UpdateBottom();
    }
    public void UpdateTop()
    {
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y + 1 * speed);
        if (rectTransform.offsetMax.y > limit)
        {
            Reset();
        }
    }

    private void Reset()
    {
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - limit);
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - limit);
    }

    public void UpdateBottom()
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y + 1 * speed);
    }
}
