using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public RectTransform InventoryPanel;
    public RectTransform LootPanel;
    public RectTransform CharacterPanel;

    private void Start()
    {
        GameEvents.current.onOpenInventory += openInventory;
    }

    public void openInventory()
    {
        InventoryPanel.gameObject.SetActive(!InventoryPanel.gameObject.activeSelf);
    }
}
