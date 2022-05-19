using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FalloffGenerator
{
    //public static float a = 3;
    //public static float b = 2.2f;
    
    
    public static float[,] GenerateFallofMap(int size, HeightMapSettings heightMapSettings, float falloffSize, float xOffset, float yOffset)
    {
        float[,] map = new float[size, size];
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = ((i / (float)size * 2 - 1)  + xOffset) * falloffSize;
                float y = ((j / (float)size * 2 - 1)  + yOffset) * falloffSize;
                
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                
                //map[i, j] = value;

                map[i, j] = Evaluate(value, heightMapSettings.a, heightMapSettings.b);
            }
        }
        
        //Try to make alternative
        /*
        float mltp = 1f / size;
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                
                if (i >= size / 2)
                {
                    map[i, j] = i * mltp * 2 - 1;
                }
                else if (i <= size / 2)
                {
                    map[i, j] = -(i * mltp * 2 - 1);
                }
                
            }
        }
        */
        
        return map;
        
        static float Evaluate(float value, float a, float b)
        {
            return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
        }
        
    } 
    
    // Circular gradient
    /*
    public static float[,] GenerateFallofMap(int size, HeightMapSettings heightMapSettings, float falloffSize, float xOffset, float yOffset)
    {
        float[,] map = new float[size, size];
        Vector2 circleCentre = new Vector2(size * 0.5f, size * 0.5f);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float dstFromCenter = (Vector2.Distance(circleCentre, new Vector2(i, j)))/heightMapSettings.falloffCircleOffset;
                float value = ((0.5f - (dstFromCenter / size)) * heightMapSettings.falloffThresh);
                map[i, j] = Mathf.InverseLerp(1, 0,value);
            }
        }

        return map;

    }
    */
}
