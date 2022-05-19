using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Generation/HeightMap Settings")]
public class HeightMapSettings : UpdatableData
{
    public NoiseSettings noiseSettings;
    public float heightMultiplayer;
    public AnimationCurve heightCurve;
        

    [Header("FalloffMap Adjusting")]
    public bool useFalloff;
    public float a = 3;
    public float b = 2.2f;
    
    [Header("Preview Only")]
    public float falloffXoffset;
    public float falloffYoffset;
    public float falloffSize;
    public float falloffMultiplier;
    //public float falloffThresh;
    //public float falloffCircleOffset;


    public float minHeight
    {
        get
        {
            return heightMultiplayer * heightCurve.Evaluate(0);
        }
    }
    public float maxHeight
    {
        get
        {
            return heightMultiplayer * heightCurve.Evaluate(1); 
        }
    }

    #if UNITY_EDITOR

    protected override void OnValidate()
    {
        noiseSettings.ValidateValues();
        base.OnValidate();
    }
    
    #endif

}
