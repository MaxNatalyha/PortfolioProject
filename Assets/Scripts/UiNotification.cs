using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiNotification : MonoBehaviour
{
    public RectTransform notifTrans;
    public Text notifText;

    public void ShowCarSeatNotification(string text)
    {
        notifTrans.gameObject.SetActive(true);
        notifText.text = text;
    }
    
    public void HideCarSeatNotification()
    {
        notifTrans.gameObject.SetActive(false);
    }
}
