using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int maxCoins = 5;
    public float spawnRange = 10f;
    public float respawnDelay = 2f;

    private List<GameObject> activeCoins = new List<GameObject>();

    void Start()
    {
        SpawnInitialCoins();
    }

    void Update()
    {
        // Clean up destroyed coins from the list
        activeCoins.RemoveAll(coin => coin == null);

        // Respawn if under the limit
        if (activeCoins.Count < maxCoins)
        {
            StartCoroutine(SpawnCoinWithDelay(respawnDelay));
        }
    }

    void SpawnInitialCoins()
    {
        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    System.Collections.IEnumerator SpawnCoinWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Check again in case multiple coroutines fire
        if (activeCoins.Count < maxCoins)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            0.25f,
            Random.Range(-spawnRange, spawnRange)
        );

        GameObject coin = Instantiate(coinPrefab, randomPos, Quaternion.identity);
        activeCoins.Add(coin);
    }
}