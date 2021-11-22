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

    [Header("Zoom Parameters")] 
    [SerializeField] private float zoomMultiplier;
    [SerializeField] private Vector2 zoomVector2;

    // Private variables
    private RectTransform thisTransform; // Usefull for every tweening animation
    private CardData cardData;
    public bool isZoomed;

    #endregion

    #region Properties

    public CardData Data => cardData;

    public bool CanZoom { get; set; } = false;

    public Vector2 MinPosition { get; private set; }

    public Vector2 MaxPosition { get; private set; }

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

    public void InitPosition()
    {
        MinPosition = thisTransform.anchoredPosition;
        MaxPosition = MinPosition + zoomVector2;
        CanZoom = true;
    }

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

    public void MoveCardForward()
    {
        thisTransform.DOAnchorPos(MinPosition + new Vector2(0.0f, 50f), 0.1f);
    }

    public void StartZoom()
    {
        if (!isZoomed && CanZoom)
        {
            thisTransform.DOAnchorPos(MaxPosition, 0.1f);
            transform.DOScale(Vector3.one * zoomMultiplier, 0.1f);
            transform.SetAsLastSibling();
            isZoomed = true;
        }
    }

    public void EndZoom()
    {
        if (isZoomed && CanZoom)
        {
            thisTransform.DOAnchorPos(MinPosition, 0.1f);
            transform.DOScale(Vector3.one, 0.1f);
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
