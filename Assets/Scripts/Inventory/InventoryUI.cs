using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryUISlot[] inventorySlots;
    public RectTransform UISlotPref;
    //private int _slotCount;
    private RectTransform _slotHolder;

    public void Init(int slotsCount)
    {
        inventorySlots = new InventoryUISlot[slotsCount];
        _slotHolder = GetComponent<RectTransform>();

        for (int i = 0; i < slotsCount; i++)
        {
            RectTransform slot = Instantiate(UISlotPref, _slotHolder);
            
            slot.localScale = Vector3.one;
            slot.name = "InventoryCell " + i;
  
            inventorySlots[i] = slot.GetComponent<InventoryUISlot>();
        }
    }

    public void RefreshInventoryUI(List<Item> itemsList)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            inventorySlots[i].AddItem(itemsList[i]);
        }
    }
}
