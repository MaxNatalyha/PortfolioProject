using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryUISlot[] inventorySlots;
    public RectTransform UISlotPref;
    private int _slotCount = 28;
    private RectTransform _slotHolder;

    public void Start()
    {
        inventorySlots = new InventoryUISlot[_slotCount];
        _slotHolder = GetComponent<RectTransform>();

        for (int i = 0; i < _slotCount; i++)
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
