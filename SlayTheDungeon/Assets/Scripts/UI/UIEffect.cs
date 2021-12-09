using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image effectIcon;
    [SerializeField] private TextMeshProUGUI turnText;

    #endregion

    #region public Methods

    public Image EffectIcon => effectIcon;
    public TextMeshProUGUI TurnText => turnText;

    #endregion
}
