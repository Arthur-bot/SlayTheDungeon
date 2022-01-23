using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirecampManager : Singleton<FirecampManager>
{
    [SerializeField] private GameObject firecampBtns;
    private Firecamp currentFire;
    private bool isOpen;

    public Firecamp CurrentFire { get => currentFire; set => currentFire = value; }
    public bool IsOpen { get => isOpen; set => isOpen = value; }

    public void OpenFirecamp()
    {
        if (!currentFire.IsUsed)
        {
            firecampBtns.SetActive(true);
            isOpen = true;
        }
    }
    public void ExitFireCamp(bool consume)
    {
        currentFire.IsUsed = consume;
        firecampBtns.SetActive(false);
        isOpen = false;
        if (consume) currentFire.Unlit();
    }
}
