using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    float deltaTime = 0.0f;
    
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        //style.alignment = TextAnchor.UpperCenter;

        style.fontSize = h * 2 / 40;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        
        if (fps > 59)
        {
            style.normal.textColor = Color.green;
        } else if (fps < 59 && fps > 30)
        {
            style.normal.textColor = Color.yellow;
        }
        else if (fps < 30)
        {
            style.normal.textColor = Color.red;
        }
        
        //style.normal.textColor = new Color(1f, 0.0f, 0.0f, 1.0f);
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}

