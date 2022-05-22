using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandPreview))]
public class IslandPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IslandPreview islandPreview = (IslandPreview)target;

        if (DrawDefaultInspector())
        {
            if (islandPreview.autoUpdate)
            {
                islandPreview.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            islandPreview.DrawMapInEditor();
        }
    }
}
