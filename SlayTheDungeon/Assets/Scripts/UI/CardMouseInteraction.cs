using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMouseInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Fields

    private HandAnimation handAnimation;
    private RectTransform thisTransform;
    private Hand hand;
    private TargetingSystem targetingSystem;
    private GameManager gameManager;
    private CardData cardData;
    private CardUI cardUI;

    #endregion

    #region Protected Methods

    private void Awake()
    {
        thisTransform = GetComponent<RectTransform>();

        var gameUI = GameUI.Instance;
        handAnimation = gameUI.HandAnimation;
        hand = gameUI.PlayerHand;
        targetingSystem = TargetingSystem.Instance;
        gameManager = GameManager.Instance;
        cardUI = GetComponent<CardUI>();
    }

    protected void Start()
    {
        cardData = cardUI.Data;
    }

    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameManager.IsPlayerTurn)
        {
            handAnimation.MemorizeCardGeometry(thisTransform);
            // Memorize the position in hand if need to get it back
            if (cardData.NeedTarget())
            {
                handAnimation.MoveCardForward(thisTransform);
            }
            else
            {
                targetingSystem.SetTargetMode(TargetingSystem.TargetMode.WithoutTarget);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameManager.IsPlayerTurn)
        {
            cardUI.EndZoom();
            if (cardData.NeedTarget())
            {
                cardUI.Highlight(Color.white);
                targetingSystem.StartTargeting(transform.position.x, transform.position.y);
            }
            else
            {
                if (targetingSystem.MouseIsOutsideHand)
                {
                    cardUI.Highlight(Color.yellow);
                }
                else
                {
                    cardUI.Highlight(Color.white);
                }
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameManager.IsPlayerTurn)
        {
            cardUI.DisableHighlight();
            handAnimation.ReturnHandAnimation(thisTransform);
            if (cardData.NeedTarget() && targetingSystem.getTarget() != null || !cardData.NeedTarget() && targetingSystem.MouseIsOutsideHand)
            {
                hand.RemoveCard(thisTransform);
                cardData.Use();
            }
            targetingSystem.StopTargeting();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardUI.Highlight(Color.white);
        cardUI.StartZoom();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        cardUI.EndZoom();
        cardUI.DisableHighlight();
    }
}