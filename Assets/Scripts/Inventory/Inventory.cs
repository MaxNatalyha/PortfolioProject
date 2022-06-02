using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum ItemTypes
{
    Wood,
    Stone
}

public class Inventory : MonoBehaviour
{
    public List<Item> ItemsList = new List<Item>();
    public InventoryUI inventoryUI;
    public int resourcesLimit;

    private int _inventorySize = 28;

    public void Start()
    {
        GameEvents.current.AddItemInInventory += TryAddItem;
        GameEvents.current.RemoveLastItemFromInventory += RemoveLastItem;
        GameEvents.current.RemoveCurrentItemFromInventory += RemoveCurrentItem;

        inventoryUI.Init(_inventorySize);
    }

    private void AddItemToInventory(Item item)
    {
        ItemsList.Add(item);
        inventoryUI.AddItemToUI(item);
        
        GameEvents.current.ShowNotification("Добавлен предмет: " + item.itemType);
    }

    public void RemoveLastItem()
    {
        if (!(ItemsList.Count == 0))
        {
            DropItem(ItemsList[ItemsList.Count - 1]);
            GameEvents.current.ShowNotification(ItemsList[ItemsList.Count - 1].GetInfo());
            
            inventoryUI.RemoveItemFromUI(ItemsList[ItemsList.Count - 1]);
            ItemsList.RemoveAt(ItemsList.Count - 1);

        }
    }
    
    public void RemoveCurrentItem(Item item)
    {
        DropItem(item);
        ItemsList.Remove(item);
        //inventoryUI.RefreshInventoryUI(ItemsList);
        GameEvents.current.ShowNotification(item.GetInfo());
    }
    
    public void TryAddItem(Item item)
    {
        if (ItemsList.Count >= _inventorySize)
        {
            GameEvents.current.ShowNotification("Инвентарь заполнен");
            DropItem(item);
            return;
        } 
        else
        {
            if (item.isStackable)
            {
                if(AddToStack(item))
                    Debug.Log("Кладем в стак");
                else
                    AddItemToInventory(item);
            }
        }
        
        Debug.Log("Количество в инвентаре: " + ItemsList.Count);
    }

    public bool AddToStack(Item item)
    {
        foreach (var itemInInventory in ItemsList)
        {
            if (itemInInventory.itemType == item.itemType && itemInInventory.count < GetStackLimit(item))
            {
                if (itemInInventory.count + item.count > GetStackLimit(item))
                {
                    int previousCount = itemInInventory.count;
                    itemInInventory.count = GetStackLimit(item);
                    item.count -= GetStackLimit(item) - previousCount;
                    AddItemToInventory(item);
                }
                else
                { 
                    itemInInventory.count += item.count;
                    inventoryUI.RefreshSlotItem(itemInInventory);
                }
                return true;
            }
        }
        return false;
    }

    private void DropItem(Item item)
    {
        GameObject dropedItem = Instantiate(InventoryAssets.GetItemPrefab(item), transform.position + (transform.forward * 3), Quaternion.identity);
        dropedItem.AddComponent<ItemWorld>().SetItem(item);
    }

    private int GetStackLimit(Item item)
    {
        switch (item.itemType)
        {
            default:
            case ItemTypes.Stone: return resourcesLimit;
            case ItemTypes.Wood: return resourcesLimit;
        }
    }
}
