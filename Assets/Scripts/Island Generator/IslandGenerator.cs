using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    public Material mapMaterial;
    public IslandPreview islandPreview;
    public SpawnScatterObjects spawnScatterObjects;

    [Header("Settings Objects")]
    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureSettings;

    [Header("Prefabs")]
    public GameObject underwaterGO;
    public GameObject waterGO;
    
    private Transform waterChunksHolder;
    private Transform islandChunksHolder;

    private void Awake()
    {
        InventoryAssets.LoadResoursec();
    }

    private void Start()
    {
        if(islandPreview!=null)
            islandPreview.gameObject.SetActive(false);

        waterChunksHolder = new GameObject().transform;
        waterChunksHolder.name = "Water Chunks Holder";
        waterChunksHolder.transform.localPosition = Vector3.zero;
        islandChunksHolder = new GameObject().transform;
        islandChunksHolder.name = "Island Chunks Holder";
        islandChunksHolder.transform.localPosition = Vector3.zero;
        
        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        
        
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                if(x < -1 || x > 1 || y < -1 || y > 1 )
                    GenerateUnderwaterSurfaces(new Vector2(x, y));
                else
                {
                    IslandChunk islandChunk = new IslandChunk(new Vector2(x, y), islandChunksHolder, heightMapSettings,
                        meshSettings, mapMaterial, spawnScatterObjects, textureSettings);
                }
            }
        }
        
        
        CreateWaterSurface();
        
        //islandChunk.Load();
    }

    private void CreateWaterSurface()
    {
        GameObject waterSurface = Instantiate(waterGO, Vector3.zero, Quaternion.identity);
        waterSurface.transform.parent = waterChunksHolder;
        float waterYpos = 17f;
        waterSurface.transform.localPosition = new Vector3(0f, waterYpos, 0f);
        waterSurface.transform.localScale = new Vector3(meshSettings.meshWorldSize, 1f, meshSettings.meshWorldSize) / 10f * 5f;
    }

    private void GenerateUnderwaterSurfaces(Vector2 coord)
    {
        GameObject underwaterPlane = Instantiate(underwaterGO, Vector3.zero, Quaternion.identity);
        Vector2 position = coord * meshSettings.meshWorldSize;
        underwaterPlane.transform.parent = waterChunksHolder;
        underwaterPlane.transform.localPosition = new Vector3(position.x, 0f, position.y);
        underwaterPlane.transform.localScale = new Vector3(meshSettings.meshWorldSize, 1f, meshSettings.meshWorldSize) / 10f;
    }
}

public class IslandChunk
{
    public Vector2 coord;
    
    private GameObject meshObject;
    private Vector2 sampleCentre;
    
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    
    private HeightMap heightMap;
    private bool heightMapReceived;

    private HeightMapSettings heightMapSettings;
    private MeshSettings meshSettings;
    
    public IslandChunk(Vector2 coord, Transform parent, HeightMapSettings heightMapSettings, MeshSettings meshSettings, Material material, SpawnScatterObjects spawnScatterObjects, TextureData textureData)
    {
        this.coord = coord;
        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        
        sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        Vector2 position = coord * meshSettings.meshWorldSize;
        
        heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre, meshSettings, false);

        meshObject = new GameObject("Island Chunk" + position);
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshRenderer.material = material;
        
        spawnScatterObjects.SpawnScatter(heightMap, textureData, meshSettings, sampleCentre, meshObject.transform);

        meshObject.transform.position = new Vector3(position.x, 0f, position.y);
        meshObject.transform.parent = parent;
        
        
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, 0);
        meshFilter.mesh = meshData.CreateMesh();
        meshCollider.sharedMesh = meshFilter.mesh;
        
    }
    
    public void Load()
    {
        //ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero), OnHeightMapReceived);
    }
    
    /*
    void OnHeightMapReceived(object heightMapObject)
    {
        this.heightMap = (HeightMap)heightMapObject;
        heightMapReceived = true;
        
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, 0), OnMeshDataReceived);
        
        //UpdateTerrainChunk();
    }
    
    void OnMeshDataReceived(object meshDataObject)
    {
        meshFilter.mesh = ((MeshData)meshDataObject).CreateMesh();
        //hasMesh = true;

        //updateCallback();
    }
    */
}


