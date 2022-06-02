using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

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

    public Action<Item> AddItemInInventory;
    public Action RemoveLastItemFromInventory;
    public Action<Item> RemoveCurrentItemFromInventory;
    
    public Action<string> ShowNotification;

}
