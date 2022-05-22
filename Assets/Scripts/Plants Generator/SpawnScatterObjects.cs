using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScatterObjects : MonoBehaviour
{
    public List<ScatterSettings> ScattersForSpawn = new List<ScatterSettings>();

    public void SpawnScatter(HeightMap heightMap, TextureData textureData, MeshSettings meshSettings, Vector2 sampleCenter, Transform chunkParent)
    {
        foreach (var scatter in ScattersForSpawn)
        {
            GameObject ScatterHolder = new GameObject(scatter.scatterHolderName);
            ScatterHolder.transform.parent = chunkParent;
            
            List<Vector3> spawnPositions =
                Scatter.CalculatePossibleSpawnPosition(heightMap, textureData, meshSettings, scatter, sampleCenter);
            
            SpawnObjects(spawnPositions, scatter, ScatterHolder.transform);
        }
    }
    
    private void SpawnObjects(List<Vector3> spawnPositions, ScatterSettings scatterSettings, Transform parent)
    {
        foreach (var spawnPosition in spawnPositions)
        {
            GameObject Tree = Instantiate(scatterSettings.spawnObjectsPref[Random.Range(0, scatterSettings.spawnObjectsPref.Length)], spawnPosition, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            float rndScale = Random.Range(1-scatterSettings.scaleRange, 1+scatterSettings.scaleRange);
            Tree.transform.localScale = new Vector3(rndScale, rndScale, rndScale);
            Tree.transform.parent = parent;
        }
    }
}
