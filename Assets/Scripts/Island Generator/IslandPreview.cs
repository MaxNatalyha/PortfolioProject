using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPreview : MonoBehaviour
{
    public enum PreviewMode
    {
        HeigthMap,
        Mesh,
        FalloffMap,
        NoiseMapChunks,
        ColourMapChunks,
        FalloffMapChunks
    }
    
    public PreviewMode previewMode;
    
    [Header("Preview Refs")]
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public TreesGenerator treesGenerator;
    
    [Header("Settings")]
    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;
    
    [Header("Materials")]
    public Material terrainMaterial;
    public Material previewMaterial;
    
    [Header("Other")]
    [Range(0, MeshSettings.numSupportedLODSs - 1)]
    public int editorPreviewLOD;
    public bool autoUpdate;
    public List<GameObject> previewPlanes = new List<GameObject>();
    
    public void ClearPreviewPlanes()
    {
        if (previewPlanes != null)
        {
            foreach (var previewPlane in previewPlanes)
            {
                DestroyImmediate(previewPlane);
            }
            previewPlanes.Clear();
        }
    }

    public void DrawMapInEditor()
    {
        ClearPreviewPlanes();
        
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        if (previewMode == PreviewMode.FalloffMapChunks || previewMode == PreviewMode.NoiseMapChunks || previewMode == PreviewMode.ColourMapChunks)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    CreatePreviewPlane(new Vector2(x, y));
                }
            }
            textureRender.gameObject.SetActive(false);
            meshFilter.gameObject.SetActive(false);

            return;
        }
        
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine,
            meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero, meshSettings, true);

        if (previewMode == PreviewMode.HeigthMap)
            DrawTexture(TextureGenerator.TextureFromHightMap(heightMap, textureData.globalHeight));
        else if (previewMode == PreviewMode.Mesh)
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
        else if (previewMode == PreviewMode.FalloffMap)
            DrawTexture(TextureGenerator.TextureFromFalloffMap(FalloffGenerator.GenerateFallofMap(meshSettings.numVertsPerLine, heightMapSettings, heightMapSettings.falloffSize, heightMapSettings.falloffXoffset,heightMapSettings.falloffYoffset)));

        
        treesGenerator.GenerateTrees(heightMap, textureData);
        
    }

    private Color[] CreateColourMap(HeightMap heightMap, int mapSize)
    {
        Color[] colourMap = new Color[mapSize * mapSize];
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                heightMap.values[x, y] =
                    Mathf.InverseLerp(0, textureData.globalHeight, heightMap.values[x, y]);
            }
        }

        for (int y = 0; y < mapSize; y++) 
        {
            for (int x = 0; x < mapSize; x++) 
            {
                float currentHeight = heightMap.values [x, y];
                
                for (int i = 0; i < textureData.layers.Length; i++)
                {
                    if (currentHeight >= textureData.layers[i].startHeight) 
                    {
                        colourMap [y * meshSettings.numVertsPerLine + x] = textureData.layers[i].tint;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return colourMap;
    }
    
    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;
        
        textureRender.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);

    }

    private void CreatePreviewPlane(Vector2 coord)
    {
        Vector2 position = coord * meshSettings.meshWorldSize;
        Vector2 sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        Texture2D texture = Texture2D.whiteTexture;
        GameObject previewPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        previewPlanes.Add(previewPlane);
        
        Renderer planeRenderer = previewPlane.GetComponent<Renderer>();
        previewPlane.transform.parent = transform;
        previewPlane.name = "Preview Plane: " + position;
        previewPlane.transform.position = new Vector3(-position.x, 0, position.y);
        previewPlane.transform.localScale = new Vector3(meshSettings.meshWorldSize, 1, meshSettings.meshWorldSize) / 10f;
        planeRenderer.material = new Material(previewMaterial);

        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre, meshSettings, false);
        
        if (previewMode == PreviewMode.NoiseMapChunks)
            texture = TextureGenerator.TextureFromHightMap(heightMap, textureData.globalHeight);
        else if (previewMode == PreviewMode.FalloffMapChunks)
        {
            float falloffXoffset = ((float)sampleCentre.x / meshSettings.meshWorldSize * heightMapSettings.falloffMultiplier) * meshSettings.meshScale;
            float falloffYoffset = ((float)sampleCentre.y / meshSettings.meshWorldSize * heightMapSettings.falloffMultiplier) * meshSettings.meshScale;
            
            float falloffSize = 1f / 3f;
            
            texture = TextureGenerator.TextureFromFalloffMap(FalloffGenerator.GenerateFallofMap(
                meshSettings.numVertsPerLine, heightMapSettings, falloffSize,  falloffXoffset, -falloffYoffset));
        }
        else if (previewMode == PreviewMode.ColourMapChunks)
            texture = TextureGenerator.TextureFromColourMap(CreateColourMap(heightMap, meshSettings.numVertsPerLine), meshSettings.numVertsPerLine, meshSettings.numVertsPerLine);


        planeRenderer.sharedMaterial.mainTexture = texture;
    }


    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        
        textureRender.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }
    
    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }
    
    
    private void OnValidate()
    {
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }

        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }
        
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
        
    }
    
}
