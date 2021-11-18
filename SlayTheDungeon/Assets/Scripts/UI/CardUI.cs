using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    #region Fields

    [SerializeField] private Image cardArtwork;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;

    // Private variables
    private RectTransform thisTransform; // Usefull for every tweening animation

    #endregion

    #region Protected Methods

    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
    }

    #endregion

    #region Private Methods

    private void setText(string name, string description)
    {
        cardName.text = name;
        cardDescription.text = description;
    }
    private void setSprite(Sprite sprite)
    {
        cardArtwork.sprite = sprite;
    }

    #endregion

    #region Public Methods

    // Public Methods
    public void SetupCard(CardData data)
    {
        // Setup the visual of the card
        setSprite(data.Sprite);
        setText(data.CardName, data.Description);
    }

    public void StartZoom()
    {
        thisTransform.SetAsLastSibling(); // Puts the card in the foreground
        thisTransform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.2f);
    }
    public void EndZoom()
    {
        thisTransform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }
    public void HideCard()
    {
        thisTransform.DOScale(new Vector3(0f, 0f, 0f), 0.2f);
    }
    public void ShowCard()
    {
        thisTransform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }

    #endregion
}
