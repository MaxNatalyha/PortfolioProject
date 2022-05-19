using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesGenerator : MonoBehaviour
{
    public List<Vector2> TreesPos = new List<Vector2>();
    private float minGrow;
    private float maxGrow;
    
    public void GenerateTrees(HeightMap heightMap, TextureData textureData)
    {
        TreesPos.Clear();
        
        minGrow = Mathf.Lerp(heightMap.minValue, heightMap.maxValue,textureData.layers[2].startHeight);
        maxGrow = Mathf.Lerp(heightMap.minValue, heightMap.maxValue,textureData.layers[3].startHeight);

        for (int x = 0; x < heightMap.values.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.values.GetLength(1); y++)
            {
                if(heightMap.values[x,y] > minGrow && heightMap.values[x,y] < maxGrow)
                    TreesPos.Add(new Vector2(x,y));
            }
        }
        
        Debug.Log(minGrow);
        Debug.Log(maxGrow);
        //Debug.Log(heightMap.values[heightMap.values.Length/2,heightMap.values.Length/2]);
        Debug.Log(heightMap.minValue);
        Debug.Log(heightMap.maxValue);


    }

    public void OnDrawGizmos()
    {
        if (TreesPos != null)
        {
            Gizmos.color = Color.green;
            foreach (var tree in TreesPos)
            {
                Gizmos.DrawSphere(new Vector3(tree.x, 50f, tree.y), 2f);
            }
        }
    }
}
