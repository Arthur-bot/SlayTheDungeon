using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecamp : MonoBehaviour
{
    private bool isUsed;
    private FirecampManager firecampManager;

    public bool IsUsed { get => isUsed; set => isUsed = value; }
    private void Start()
    {
        firecampManager = FirecampManager.Instance;
    }

    private void OnMouseDown()
    {
        firecampManager.CurrentFire = this;
        firecampManager.OpenFirecamp();
    }
}
