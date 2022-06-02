using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemWorld : MonoBehaviour
{
    private Item item;
    
    private float height = 0.2f;
    private float period = 1;
    private Vector3 initialPosition;
    private float offset;

    private void Awake()
    {
        initialPosition = transform.position;

        offset = 1 - (Random.value * 2);
    }

    public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        Debug.Log("Выкинули: " + this.item.GetInfo());
    }

    private void Update()
    {
        transform.position = initialPosition - Vector3.up * Mathf.Sin((Time.time + offset) * period) * height;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
