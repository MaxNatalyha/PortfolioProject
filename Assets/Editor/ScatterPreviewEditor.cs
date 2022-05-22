using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScatterPreview))]
public class ScatterPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {        
        base.OnInspectorGUI();

        ScatterPreview scatterPreview = (ScatterPreview)target;
        
        if (GUILayout.Button("Visualize scatter objects"))
        {
            if(scatterPreview.previewPossiblePositions!=null)
                scatterPreview.previewPossiblePositions.Clear();
            
            scatterPreview.CalculatePreviewPossiblePosition();
            
        }
        if (GUILayout.Button("Disable visualization"))
        {
            if(scatterPreview.previewPossiblePositions!=null)
                scatterPreview.previewPossiblePositions.Clear();
        }
    }
}
