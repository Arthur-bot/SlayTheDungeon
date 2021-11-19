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
    [SerializeField] private Image cardHighlight;
    [SerializeField] private RectTransform hitbox;

    // Private variables
    private RectTransform thisTransform; // Usefull for every tweening animation
    private CardData cardData;
    private bool isZoomed;

    #endregion

    #region Properties

    public CardData Data => cardData;

    #endregion

    #region Protected Methods

    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
        DisableHighlight();
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
        cardData = data;
        setSprite(data.Sprite);
        setText(data.CardName, data.Description);
    }
    public void Highlight (Color color)
    {
        cardHighlight.enabled = true;
        cardHighlight.color = color;
    }
    public void StartZoom()
    {
        if (!isZoomed)
        {
            //thisTransform.anchoredPosition += new Vector2(0, 120);
            //hitbox.anchoredPosition -= new Vector2(0, 60);
            isZoomed = true;
            transform.localScale = Vector3.one * 2;
            hitbox.localScale = Vector3.one / 2;
            transform.SetAsLastSibling();
        }
    }
    public void EndZoom()
    {
        if (isZoomed)
        {
            //thisTransform.anchoredPosition -= new Vector2(0, 120);
            //hitbox.anchoredPosition += new Vector2(0, 60);
            hitbox.localScale = Vector3.one;
            transform.localScale = Vector3.one;
            isZoomed = false;
        }
    }
    public void DisableHighlight()
    {
        cardHighlight.enabled = false;
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
