using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemTypes
{
    Wood,
    Stone
}

public class Inventory : MonoBehaviour
{
    public List<Item> ItemsList = new List<Item>();
    public InventoryUI inventoryUI;

    public void Start()
    {
        HUDEvents.current.onAddingItem += AddItem;
    }

    public void AddItem(Item item)
    {
        ItemsList.Add(item);
        inventoryUI.RefreshInventoryUI(ItemsList);
        Debug.Log(item.GetInfo());
    }
}
