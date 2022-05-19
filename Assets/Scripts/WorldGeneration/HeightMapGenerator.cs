using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings, Vector2 sampleCentre, MeshSettings meshSettings, bool isPreview)
    {
        float[,] values = Noise.GenerateNoiseMap(width, height, settings.noiseSettings, sampleCentre);

        Vector2 falloffOffset = Vector2.zero;
        float falloffSize = 1f;

        if (!isPreview)
        {
            falloffOffset.x = sampleCentre.x / meshSettings.meshWorldSize * 2 * meshSettings.meshScale;
            falloffOffset.y = sampleCentre.y / meshSettings.meshWorldSize * 2 * meshSettings.meshScale;
            
            falloffSize = 1f / 3f;
        }
        
        float[,] falloffMap = FalloffGenerator.GenerateFallofMap(width, settings, falloffSize, falloffOffset.x, -falloffOffset.y);
        
        AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (settings.useFalloff)
                {
                    values[i, j] = Mathf.Clamp01(values [i, j] - falloffMap [i, j]);
                }
                
                values[i, j] *= heightCurve_threadsafe.Evaluate(values[i, j]) * settings.heightMultiplayer;

                if (values[i, j] > maxValue)
                {
                    maxValue = values[i, j];
                }

                if (values[i, j] < minValue)
                {
                    minValue = values[i, j];
                }

            }
        }
        return new HeightMap(values, minValue, maxValue);
    }
}

public struct HeightMap
{
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;
    
    public HeightMap(float[,] values, float minValue, float maxValue)
    {
        this.values = values;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}