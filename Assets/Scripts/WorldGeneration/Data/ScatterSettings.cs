using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LayerTypes
{
    Water,
    Sand,
    Grass,
    Hill,
    Cliffs,
    Snow
};

[CreateAssetMenu(menuName = "World Generation/Scatter Settings")]
public class ScatterSettings : ScriptableObject
{
    public string scatterHolderName;
    [Header("Noise Map settings")]
    public NoiseSettings noiseSettings;

    [Header("Objects to spawn prefabs")] 
    public GameObject[] spawnObjectsPref;

    [Header("Generate Adjusting")] 
    [Tooltip("Inclusive start layer")]
    public LayerTypes startLayer;
    [Tooltip("Exclusive end layer")]
    public LayerTypes endLayer;
    public bool inclusiveLastLayer;
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
    public Vector3 previewBoxSize;
}
