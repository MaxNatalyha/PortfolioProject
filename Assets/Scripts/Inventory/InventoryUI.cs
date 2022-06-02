using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryUISlot[] inventorySlots;
    
    [Header("UI Prefabs")]
    public RectTransform UISlotPref;
    public RectTransform UIItemIconPref;

    public Color slotMainColor;
    public Color slotHoverColor;

    private RectTransform _slotHolder;


    public void Init(int slotsCount)
    {
        inventorySlots = new InventoryUISlot[slotsCount];
        _slotHolder = GetComponent<RectTransform>();

        for (int i = 0; i < slotsCount; i++)
        {
            RectTransform slot = Instantiate(UISlotPref, _slotHolder);
            slot.localScale = Vector3.one;
            slot.name = "InventorySlot " + i;
            
            inventorySlots[i] = slot.GetComponent<InventoryUISlot>();
            inventorySlots[i].Init(slotMainColor, slotHoverColor);
        }

    }

    public void AddItemToUI(Item item)
    {
        // РАЗОБРАТЬСЯ С ОТКЛЮЧЕНИЕМ ГРИДА
        var grid = GetComponent<GridLayoutGroup>();
        grid.enabled = false;
        //
        
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i]._uiItem == null)
            {
                item.icon = InventoryAssets.GetItemIcon(item);
                inventorySlots[i].CreateItemUI(item, UIItemIconPref);
                return;
            }
        }
    }

    public void RemoveItemFromUI(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i]._uiItem != null && inventorySlots[i]._uiItem.item == item)
            {
                inventorySlots[i]._uiItem.DeleteIcon();
                Debug.Log("Нашли и удалили");
                return;
            }
        }
    }
    
    public void RefreshSlotItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        { 
            if (inventorySlots[i]._uiItem != null && inventorySlots[i]._uiItem.item == item)
            {
                inventorySlots[i]._uiItem.RefreshInfo();
                return;
            }
        }
    }
}
