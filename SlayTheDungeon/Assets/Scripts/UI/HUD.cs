using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image characterSprite;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private List<UIEffect> uiEffects;
    [SerializeField] private UIEffect uiNextAction;

    private CharacterData data;
    private List<UIVisualEffect> visualEffects;

    #endregion

    #region Properties

    public Image CharacterSprite => characterSprite;

    #endregion

    #region Public Methods

    public void Init(CharacterData data)
    {
        this.data = data;
        UpdateHUD(data);
    }

    public void UpdateHUD(CharacterData data)
    {
        healthText.text = data.Stats.CurrentHealth + "/" + data.Stats.StatsCopy.Health;
        armorText.text = data.Stats.CurrentArmor.ToString();
        armorText.gameObject.SetActive(data.Stats.CurrentArmor > 0);
        healthSlider.value = data.Stats.CurrentHealth / (float)data.Stats.StatsCopy.Health;

        int elementalEffectCount = data.Stats.ElementalEffects.Count;
        for (int i = 0; i < elementalEffectCount; ++i)
        {
            var effect = data.Stats.ElementalEffects[i];

            uiEffects[i].EffectIcon.sprite = effect.EffectSprite;
            uiEffects[i].ValueText.text = effect.Timer.ToString();
            uiEffects[i].gameObject.SetActive(true);
        }

        visualEffects = new List<UIVisualEffect>();
        if (data.Stats.CurrentFury > 0)
        {
            visualEffects.Add(new UIVisualEffect { sprite = DataBase.Instance.FuryIcon, value = data.Stats.CurrentFury });
        }
        
        for (int i = 0; i < visualEffects.Count; ++i)
        {
            uiEffects[i + elementalEffectCount].EffectIcon.sprite = visualEffects[i].sprite;
            uiEffects[i + elementalEffectCount].ValueText.text = visualEffects[i].value.ToString();
            uiEffects[i + elementalEffectCount].gameObject.SetActive(true);
        }

        for (int i = elementalEffectCount + visualEffects.Count ; i < uiEffects.Count; ++i)
        {
            uiEffects[i].gameObject.SetActive(false);
        }
    }

    public void NextAction(CardEffect action)
    {
        if (!uiNextAction.gameObject.activeSelf)
        {
            uiNextAction.gameObject.SetActive(true);
        }

        uiNextAction.EffectIcon.sprite = action.GetIcon();
        uiNextAction.ValueText.text = action.GetEffectValue().ToString();
    }

    public void HideAction()
    {
        uiNextAction.gameObject.SetActive(false);
    }

    #endregion

}
