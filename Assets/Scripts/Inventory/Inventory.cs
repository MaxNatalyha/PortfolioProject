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
    private int _inventorySize = 2;
    private ItemTypes itemTypes;

    public void Start()
    {
        HUDEvents.current.onAddingItem += TryAddItem;
        inventoryUI.Init(_inventorySize);
    }

    public void TryAddItem(Item item)
    {
        if (ItemsList.Count >= _inventorySize)
        {
            Debug.Log("Нет места");
            //выбросить на землю и заспавнить 3д префаб
            return;
        }
        
        
        
        ItemsList.Add(item);
        inventoryUI.RefreshInventoryUI(ItemsList);
        Debug.Log(item.GetInfo());
    }

    private void CheckStack(Item item)
    {
        foreach (var itemInInvenotry in ItemsList)
        {
            
        }
    }

    public int GetItemLimit(Item item)
    {
        switch (itemTypes)
        {
            
        }
        return 1;
    }
}
