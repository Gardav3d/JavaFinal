using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
public class NatureGenerator : MonoBehaviour
{
    private static NatureGenerator instance;
 
    [SerializeField] private bool alignToGround;
    
    [SerializeField] private List<GameObject> naturePrefabs;
 
    [SerializeField, Range(0, 100)] private float spawnChance;
    [SerializeField] private float spawnDistance, minSpawnHeight, maxSpawnHeight;
    [SerializeField] private LayerMask groundLayer;
    
    private void Awake()
    {
        instance = this;
    }
 
    public static void SpawnNature()
    {
        instance.ClearNature();
        
        float xPos = instance.transform.position.x;
        float zPos = instance.transform.position.z;
        for (float x = xPos; x < xPos + MeshGenerator.GetSize(); x += instance.spawnDistance)
        {
            for (float z = zPos; z < zPos + MeshGenerator.GetSize(); z += instance.spawnDistance)
            {
                if (Random.Range(0, 100) < instance.spawnChance)
                {
                    if (!instance.GetHeight(x, z, out float y, out Vector3 normal))
                        continue;
                    if (y < instance.minSpawnHeight || y > instance.maxSpawnHeight)
                        continue;
                    
                    int index = Random.Range(0, instance.naturePrefabs.Count);
                    GameObject spawned = Instantiate(instance.naturePrefabs[index], new Vector3(x, y, z), Quaternion.identity, instance.transform);
                    spawned.transform.rotation = Quaternion.FromToRotation(spawned.transform.up, normal) * spawned.transform.rotation;
                }
            }
        }
    }
 
    private bool GetHeight(float x, float z, out float y, out Vector3 normal)
    {
        if (!Physics.Raycast(new Vector3(x, 999, z), Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            normal = Vector3.zero;
            y = 999999;
            return false;
        }
 
        normal = hit.normal;
        y = hit.point.y;
        return true;
    }
 
    private void ClearNature()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}