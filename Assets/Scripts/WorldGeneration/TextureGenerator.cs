using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D TextureFromHightMap(HeightMap heightMap, float globalHeigth)
    {
        int width = heightMap.values.GetLength(0);
        int height = heightMap.values.GetLength(1);
        
        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap [y * width + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(0, globalHeigth,heightMap.values[x, y]));
            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }

    public static Texture2D TextureFromFalloffMap(float[,] falloffMap)
    {
        int width = falloffMap.GetLength(0);
        int height = falloffMap.GetLength(1);
        
        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] =
                    Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(0, 1, falloffMap[x, y]));
            }
        }
        
        return TextureFromColourMap(colourMap, width, height);
    }
}
