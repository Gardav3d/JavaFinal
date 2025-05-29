// SpawnManager.cs
using UnityEngine;
using FishNet.Object;
using System.Collections.Generic;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points set in SpawnManager.");
            return null;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
