using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Scatter : MonoBehaviour
{
    [Header("Noise Map settings")]
    public HeightMapSettings noiseSettings;

    [Header("Trees prefabs")] 
    public GameObject[] spawnObjectsPref;

    [Header("Generate Adjusting")] 
    [Range(0, 1)]
    public float positionOffset;
    [Range(0,.5f)]
    public float scaleRange;
    public float innerThreshold;
    public float outerThreshold;
    [Range(0,1)]
    public float density;
    [Range(0,1)]
    public float noiseStrength;
    
    [Header("Preview")]
    public Color previewColor;

    
    public List<Vector3> CalculatePossibleSpawnPosition(HeightMap heightMap, TextureData textureData, MeshSettings meshSettings, LayerTypes currentType)
    {
        List<Vector3> TreesPosiblePosition = new List<Vector3>();
        
        int mapLength = heightMap.values.GetLength(0);
        float[,] noiseTreesMap = Noise.GenerateNoiseMap(mapLength, mapLength, noiseSettings.noiseSettings, Vector2.zero);

        float minGrow = 0f;
        float maxGrow = 0f;
        
        int borderedSize = heightMap.values.GetLength(0);
        int meshSize = borderedSize - 2*1;
        int meshSizeUnsimplified = borderedSize - 2;
        
        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

        for (int i = 0; i < textureData.layers.Length-1; i++)
        {
            if (currentType == textureData.layers[i].layerType)
            {
                minGrow = Mathf.Lerp(0f, textureData.globalHeight,textureData.layers[i].startHeight) + outerThreshold;
                maxGrow = Mathf.Lerp(0f, textureData.globalHeight,textureData.layers[i+1].startHeight) - innerThreshold;
            }
        }

        for (int x = 0; x < heightMap.values.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.values.GetLength(1); y++)
            {
                if (heightMap.values[x, y] > minGrow && heightMap.values[x, y] < maxGrow && noiseTreesMap[x,y] > noiseStrength && RandomDensity())
                {
                    Vector2 percent = new Vector2((x - 1) / (float)meshSize, (y - 1) / (float)meshSize);
                    
                    Vector3 treePosition = new Vector3((topLeftX + percent.x * meshSizeUnsimplified) * meshSettings.meshScale, heightMap.values[x,y], (topLeftZ - percent.y * meshSizeUnsimplified) * meshSettings.meshScale); 
                    TreesPosiblePosition.Add(treePosition);
                }
                
            }
        }
        return TreesPosiblePosition;
    }

    public void SpawnObjects(List<Vector3> spawnPositions, GameObject[] spawnObjects)
    {
        GameObject treesHolder = new GameObject();
        treesHolder.name = "Trees Holder";
        
        foreach (var spawnPosition in spawnPositions)
        {
            GameObject Tree = Instantiate(spawnObjectsPref[Random.Range(0, spawnObjectsPref.Length-1)], spawnPosition, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            float rndScale = Random.Range(1-scaleRange, 1+scaleRange);
            Tree.transform.localScale = new Vector3(rndScale, rndScale, rndScale);
            Tree.transform.parent = treesHolder.transform;
        }
    }

    private bool RandomDensity()
    {
        float random = Random.Range(0f, 1f);
        return random < density;
    }

    public void OnDrawGizmos()
    {
        /*
        
        if (TreesPosiblePosition != null)
        {
            Gizmos.color = previewColor;
            foreach (var treePosition in TreesPosiblePosition)
            {
                Gizmos.DrawCube(new Vector3(treePosition.x, treePosition.y + 4f, treePosition.z), new Vector3(2f,8f,2f));
            }
        }
        */
    }
}
