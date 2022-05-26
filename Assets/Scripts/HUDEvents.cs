using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HUDEvents : MonoBehaviour
{
    public static HUDEvents current;

    private void Awake()
    {
        current = this;
    }
    
    public event Action onOpenInventory;
    public void OpenInventory()
    {
        if (onOpenInventory != null)
            onOpenInventory();
    }
}
