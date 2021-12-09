using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : Singleton<TargetingSystem>
{
    #region Fields

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask outsideZoneMask;
    [SerializeField] GameObject targetingArrow;

    private CharacterData target;
    private TargetMode targetMode;
    private bool mouseOutsideHand;

    public bool MouseIsOutsideHand { get => mouseOutsideHand; set => mouseOutsideHand = value; }

    #endregion

    private void Update()
    {
        RaycastHit2D hit;
        switch (targetMode)
        {
            case TargetMode.None:
                break;
            case TargetMode.SingleTarget:
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, targetMask);

                if (hit.collider != null && hit.collider.TryGetComponent(out CharacterData data))
                {
                    SetTarget(data);

                    if (!target.IsTargeted)
                        target.IsTargeted = true;
                }
                else
                {
                    if (target == null) break;

                    if (target.IsTargeted)
                        target.IsTargeted = false;

                    target = null;
                }
                break;
            case TargetMode.WithoutTarget:
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, outsideZoneMask);
                
                mouseOutsideHand = hit.collider != null;
                break;

        }
    }

    // Public Methods
    public void SetTarget(CharacterData newTarget)
    {
        target = newTarget;
    }
    public void StartTargeting(float x, float y)
    {
        targetingArrow.SetActive(true);
        targetingArrow.transform.position = new Vector2(x, y);
        SetTargetMode(TargetMode.SingleTarget);
    }

    public void StopTargeting()
    {
        targetingArrow.SetActive(false);
        SetTargetMode(TargetMode.None);
        target = null;
    }
    // Getter
    public CharacterData getTarget() { return target; }

    public void SetTargetMode(TargetMode mode)
    {
        targetMode = mode;
    }

    public enum TargetMode
    {
        None,
        WithoutTarget,
        SingleTarget
    }
}
