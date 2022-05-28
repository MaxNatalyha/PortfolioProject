using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Resourсes")]
public class Item : ScriptableObject
{
    public Resources resourсe;
    public int count;
    public bool isStackable;
}
