using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item
{
    public ItemTypes itemType;
    public Sprite icon;
    public int count;
    public bool isStackable;
    
    public Item(ItemTypes type, int count, bool isStackable)
    {
        itemType = type;
        this.count = count;
        this.isStackable = isStackable;
    }

    public string GetInfo()
    {
        return "Это: " + itemType + ", в количестве: " + count;
    }
}
