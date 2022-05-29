using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    private Image icon;
    private Text count;

    private void Awake()
    {
        icon = GetComponentInChildren<Image>();
        count = GetComponentInChildren<Text>();
    }

    public void AddItem(Item item)
    {
        count.text = item.count.ToString();
        icon.sprite = item.icon;
    }
    
}
