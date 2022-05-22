using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Scatter
{
    public static List<Vector3> CalculatePossibleSpawnPosition(HeightMap heightMap, TextureData textureData, MeshSettings meshSettings, ScatterSettings scatterSettings, Vector2 sampleCenter)
    {
        List<Vector3> PossiblePositions = new List<Vector3>();
        
        int mapLength = heightMap.values.GetLength(0);
        float[,] noiseTreesMap = Noise.GenerateNoiseMap(mapLength, mapLength, scatterSettings.noiseSettings, sampleCenter);

        float minGrow = 0f;
        float maxGrow = 0f;
        
        int borderedSize = heightMap.values.GetLength(0);
        int meshSize = borderedSize - 2*1;
        int meshSizeUnsimplified = borderedSize - 2;
        
        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

        for (int i = 0; i < textureData.layers.Length; i++)
        {
            if (textureData.layers[i].layerType == scatterSettings.startLayer)
            {
                minGrow = Mathf.Lerp(0f, textureData.globalHeight, textureData.layers[i].startHeight) + scatterSettings.outerThreshold;
            }
            
            if (textureData.layers[i].layerType == scatterSettings.endLayer)
            {
                maxGrow = Mathf.Lerp(0f, textureData.globalHeight,textureData.layers[i].startHeight) - scatterSettings.innerThreshold;
                
                if(scatterSettings.inclusiveLastLayer)
                    maxGrow = Mathf.Lerp(0f, textureData.globalHeight,1);
            }
        }

        for (int x = 0; x < borderedSize; x++)
        {
            for (int y = 0; y < borderedSize; y++)
            {
                if (heightMap.values[x, y] > minGrow && heightMap.values[x, y] < maxGrow && noiseTreesMap[x,y] > scatterSettings.noiseStrength && RandomDensity(scatterSettings.density))
                {
                    Vector2 percent = new Vector2((x - 1) / (float)meshSize, (y - 1) / (float)meshSize);
                    
                    Vector3 position = new Vector3((topLeftX + percent.x * meshSizeUnsimplified) * meshSettings.meshScale, heightMap.values[x,y], (topLeftZ - percent.y * meshSizeUnsimplified) * meshSettings.meshScale); 
                    PossiblePositions.Add(position);
                }
                
            }
        }
        return PossiblePositions;
    }
    
    private static bool RandomDensity(float density)
    {
        float random = Random.Range(0f, 1f);
        return random < density;
    }
}
