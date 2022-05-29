using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public ItemTypes itemType;
    public Sprite icon;
    public int count;
    public bool isStackable;
    
    /*
    public Item(ItemTypes type, int count, bool isStackable)
    {
        _itemType = type;
        _count = count;
        _isStackable = isStackable;
    }*/

    public string GetInfo()
    {
        return "Это: " + itemType + ", в количестве: " + count;
    }
}
