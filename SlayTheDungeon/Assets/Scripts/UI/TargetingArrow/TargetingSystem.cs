using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : Singleton<TargetingSystem>
{
    #region Fields

    [SerializeField] private LayerMask targetMask;
    [SerializeField] GameObject targetingArrow;

    private CharacterData target;
    private bool isTargeting;
    private GameManager gameManager;

    #endregion

    private void Update()
    {
        // if targetting, look for a target
        if (isTargeting)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, targetMask);
            if (hit.collider != null)
            {
                SetTarget(hit.collider.GetComponent<CharacterData>());
            }
            else
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }
    }

    // Public Methods
    public void SetTarget(CharacterData newTarget)
    {
        if (isTargeting)
        {
            target = newTarget;
        }
    }
    public void StartTargeting(float x, float y)
    {
        targetingArrow.SetActive(true);
        targetingArrow.transform.position = new Vector2(x, y);
        isTargeting = true;
    }

    public void StopTargeting()
    {
        targetingArrow.SetActive(false);
        isTargeting = false;
    }
    // Getter
    public CharacterData getTarget() { return target; }
}
