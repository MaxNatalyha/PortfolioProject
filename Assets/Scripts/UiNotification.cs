using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiNotification : MonoBehaviour
{
    public RectTransform notifTrans;
    public Text notifText;

    public void ShowCarSeatNotification()
    {
        notifTrans.gameObject.SetActive(true);
        notifText.text = "Press F to pay respect";
    }
    
    public void HideCarSeatNotification()
    {
        notifTrans.gameObject.SetActive(false);
    }
}
