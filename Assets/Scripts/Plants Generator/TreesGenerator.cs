using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesGenerator : MonoBehaviour
{
    public List<Vector3> TreesPosiblePosition = new List<Vector3>();
    public HeightMapSettings noiseSettings;
    public float innerThreshold;
    public float outerThreshold;
    public Color previewColor;
    
    private float minGrow;
    private float maxGrow;
    private int mapLength;
    private float[,] noiseTreesMap;
    
    public void GenerateTrees(HeightMap heightMap, TextureData textureData, MeshSettings meshSettings)
    {
        TreesPosiblePosition.Clear();

        mapLength = heightMap.values.GetLength(0);

        noiseTreesMap = Noise.GenerateNoiseMap(mapLength, mapLength, noiseSettings.noiseSettings, Vector2.zero);
        Debug.Log(noiseTreesMap[25,25]);
        
        int borderedSize = heightMap.values.GetLength(0);
        int meshSize = borderedSize - 2*1;
        int meshSizeUnsimplified = borderedSize - 2;
        
        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;
        
        minGrow = Mathf.Lerp(0f, textureData.globalHeight,textureData.layers[2].startHeight) + outerThreshold;
        maxGrow = Mathf.Lerp(0f, textureData.globalHeight,textureData.layers[3].startHeight) - innerThreshold;

        for (int x = 0; x < heightMap.values.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.values.GetLength(1); y++)
            {
                if (heightMap.values[x, y] > minGrow && heightMap.values[x, y] < maxGrow && noiseTreesMap[x,y] > .5f)
                {
                    Vector2 percent = new Vector2((x - 1) / (float)meshSize, (y - 1) / (float)meshSize);
                    
                    Vector3 treePosition = new Vector3((topLeftX + percent.x * meshSizeUnsimplified) * meshSettings.meshScale, heightMap.values[x,y], (topLeftZ - percent.y * meshSizeUnsimplified) * meshSettings.meshScale); 

                    TreesPosiblePosition.Add(treePosition);
                }
                
            }
        }
        
        Debug.Log("Min grow: " + minGrow);
        Debug.Log("Max grow: " + maxGrow);
    }

    public void OnDrawGizmos()
    {
        /*
        for (int x = 0; x < mapLength; x++)
        {
            for (int y = 0; y < mapLength; y++)
            {
                if (noiseTreesMap[x, y] > .5f)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawWireCube(new Vector3(x, 50f, y), new Vector3(1f,3f,1f));
            }
        }
        */
        
        if (TreesPosiblePosition != null)
        {
            Gizmos.color = previewColor;
            foreach (var treePosition in TreesPosiblePosition)
            {
                Gizmos.DrawCube(new Vector3(treePosition.x, treePosition.y + 1.5f, treePosition.z), new Vector3(2f,3f,2f));
            }
        }
        
    }
}
