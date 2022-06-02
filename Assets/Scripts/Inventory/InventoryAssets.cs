using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryAssets
{

    private static readonly string uiIconsPath = "ui_icons/";
    private static readonly string itemPrefabsPath = "item_prefabs/";

    private static Dictionary<string, Sprite> itemIcons;
    private static Dictionary<string, GameObject> itemPrefabs;


    public static void LoadResoursec()
    {
        itemIcons = new Dictionary<string, Sprite>();
        itemPrefabs = new Dictionary<string, GameObject>();
        
        foreach (var icon in Resources.LoadAll<Sprite>(uiIconsPath))
        {
            itemIcons.Add(icon.name, icon);
        }
        
        foreach (var prefab in Resources.LoadAll<GameObject>(itemPrefabsPath))
        {
            itemPrefabs.Add(prefab.name, prefab);
        }
    }

    public static GameObject GetItemPrefab(Item item)
    {
        string name = item.itemType.ToString() + "_pref";
        if(!itemPrefabs.ContainsKey(name))
            Debug.LogError(string.Format("{0} no found!", name));
        
        return itemPrefabs[name];
    }
    
    public static Sprite GetItemIcon(Item item)
    {
        string name = item.itemType.ToString() + "_icon";
        if(!itemIcons.ContainsKey(name))
            Debug.LogError(string.Format("{0} no found!", name));
        
        return itemIcons[name];
    }
}
