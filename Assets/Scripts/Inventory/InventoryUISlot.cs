using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public Image icon;
    public Text count;

    private void Start()
    {
        RemoveItem();
    }

    public void AddItem(Item item)
    {
        count.text = item.count.ToString();
        icon.sprite = item.icon;
        icon.enabled = true;
        count.enabled = true;

    }

    public void RemoveItem()
    {
        count.enabled = false;
        icon.enabled = false;
    }
    
}
