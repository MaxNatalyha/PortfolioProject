using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scatter))]
public class ScatterEditor : Editor
{
    /*
    private void OnSceneGUI()
    {
        Scatter scatter = (Scatter)target;
        Handles.color = scatter.previewColor;
        if (scatter.previewPossiblePositions != null)
        {
            foreach (var previewPos in scatter.previewPossiblePositions)
            {
                Handles.DrawWireCube(previewPos, new Vector3(3,6,3));
            }
        }
    }
    */
}
