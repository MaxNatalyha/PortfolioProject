using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Item item;
    
    private RectTransform _rect;
    
    private Image _uiImage;
    private Text _count;
    
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    public void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rect = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _uiImage = GetComponent<Image>();
        _count = GetComponentInChildren<Text>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        RefreshInfo();
    }

    public void RefreshInfo()
    {
        _count.text = item.count.ToString();
        _uiImage.sprite = item.icon;
    }

    public void RemoveFromSlot()
    {
        InventoryUISlot inventoryUISlot = _rect.parent.GetComponent<InventoryUISlot>();
        inventoryUISlot._uiItem = null;
    }

    public void DeleteIcon()
    {
        RemoveFromSlot();
        Destroy(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
        Transform slotTrans = _rect.parent;
        slotTrans.SetAsLastSibling();
        Debug.Log("Взяли");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _rect.anchoredPosition = Vector2.zero;
    }
}
