using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scatter))]
public class ScatterEditor : Editor
{
    private void OnSceneGUI()
    {
        Scatter scatter = (Scatter)target;
    }
}
