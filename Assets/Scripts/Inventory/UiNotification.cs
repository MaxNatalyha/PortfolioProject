using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiNotification : MonoBehaviour
{
    public RectTransform notifTrans;
    public Text notifText;

    private void Start()
    {
        GameEvents.current.ShowNotification += ShowNotification;
    }

    private void ShowNotification(string text)
    {
        notifTrans.gameObject.SetActive(true);
        notifText.text = text;

        StartCoroutine(HideNotification());
    }
    
    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(.5f);
        
        notifTrans.gameObject.SetActive(false);
    }
}
