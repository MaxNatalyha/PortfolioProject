using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPreview : MonoBehaviour
{
    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;
    [Header("Scatter Settings")] 
    public ScatterSettings scatterSettings;

    public List<Vector3> previewPossiblePositions = new List<Vector3>();


    public void CalculatePreviewPossiblePosition()
    {
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine,
            meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero, meshSettings, true);

        previewPossiblePositions = Scatter.CalculatePossibleSpawnPosition(heightMap, textureData, meshSettings,
            scatterSettings, Vector2.zero);
    }
    
    
    public void OnDrawGizmos()
    {
        if (previewPossiblePositions != null)
        {
            Gizmos.color = scatterSettings.previewColor;
            foreach (var position in previewPossiblePositions)
            {
                Gizmos.DrawCube(new Vector3(position.x, position.y + (scatterSettings.previewBoxSize.y / 2), position.z), scatterSettings.previewBoxSize);
            }
        }
    }
    
}
