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
    }

    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameManager.IsPlayerTurn)
        {
            // Memorize the position in hand if need to get it back
            handAnimation.MemorizeCardGeometry(thisTransform);
            handAnimation.MoveCardForward(thisTransform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameManager.IsPlayerTurn)
            targetingSystem.StartTargeting(transform.position.x, transform.position.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameManager.IsPlayerTurn)
        {
            handAnimation.ReturnHandAnimation(thisTransform);
            if (targetingSystem.getTarget() != null)
            {
                hand.RemoveCard(thisTransform);
            }
            targetingSystem.StopTargeting();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TO DO:HIGHLIGHT
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        // TO DO:DISABLE HIGHLIGHT
    }
}