using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIVisualEffect
{
    public Sprite sprite;
    public int value;
}
public class UIEffect : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image effectIcon;
    [SerializeField] private TextMeshProUGUI valueText;

    #endregion

    #region public Methods

    public Image EffectIcon => effectIcon;
    public TextMeshProUGUI ValueText => valueText;

    #endregion
}
