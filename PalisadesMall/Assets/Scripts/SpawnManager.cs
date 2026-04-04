using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private List<Transform> spawnPoints = new();

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned.");
            return null;
        }

        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index];
    }
}