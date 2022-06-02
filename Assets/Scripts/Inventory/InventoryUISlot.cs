using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Image _slotImage;
    
    [HideInInspector]
    public UIItem _uiItem;
    
    private Color _mainColor;
    private Color _hoverColor;

    public void Init(Color mainColor, Color hoverColor)
    {
        _slotImage = GetComponent<Image>();
        
        _mainColor = mainColor;
        _hoverColor = hoverColor;
        _slotImage.color = _mainColor;
    }

    public void CreateItemUI(Item item, RectTransform prefab)
    {
        RectTransform newUiItemRect = Instantiate(prefab, GetComponent<RectTransform>());
        newUiItemRect.name = item.itemType.ToString();
        
        UIItem newUiItem = newUiItemRect.GetComponent<UIItem>();
        newUiItem.SetItem(item);

        _uiItem = newUiItem;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _slotImage.color = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _slotImage.color = _mainColor;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (_uiItem == null)
        {
            var otherItemTransform = eventData.pointerDrag.transform;
            otherItemTransform.GetComponent<UIItem>().RemoveFromSlot();
            otherItemTransform.SetParent(transform);
            _uiItem = otherItemTransform.GetComponent<UIItem>();
            
            Debug.Log(otherItemTransform.name);
        }
    }
}
