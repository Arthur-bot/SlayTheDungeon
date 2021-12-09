using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image characterSprite;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private List<UIEffect> uiEffects;

    #endregion

    #region Properties

    public Image CharacterSprite => characterSprite;

    #endregion

    #region Public Methods

    public void UpdateHUD(CharacterData data)
    {
        healthText.text = data.Stats.CurrentHealth + "/" + data.Stats.StatsCopy.Health;
        armorText.text = data.Stats.CurrentArmor.ToString();
        armorText.gameObject.SetActive(data.Stats.CurrentArmor > 0);
        healthSlider.value = data.Stats.CurrentHealth / (float)data.Stats.StatsCopy.Health;

        int timedEffectCount = data.Stats.TimedModifierStack.Count;
        for (int i = 0; i < timedEffectCount; ++i)
        {
            var effect = data.Stats.TimedModifierStack[i];

            uiEffects[i].EffectIcon.sprite = effect.EffectSprite;
            uiEffects[i].TurnText.text = effect.Timer.ToString();
            uiEffects[i].gameObject.SetActive(true);
        }

        int elementalEffectCount = data.Stats.ElementalEffects.Count;
        for (int i = 0; i < elementalEffectCount; ++i)
        {
            var effect = data.Stats.ElementalEffects[i];

            uiEffects[i].EffectIcon.sprite = effect.EffectSprite;
            uiEffects[i].TurnText.text = effect.Timer.ToString();
            uiEffects[i].gameObject.SetActive(true);
        }

        for (int i = timedEffectCount + elementalEffectCount; i < uiEffects.Count; ++i)
        {
            uiEffects[i].gameObject.SetActive(false);
        }
    }

    #endregion

}
