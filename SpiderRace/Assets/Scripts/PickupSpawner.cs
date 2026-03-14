using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickupSpawner : MonoBehaviour
{
    [Header("Pickup Options")]
    [SerializeField] private List<GameObject> pickupPrefabs = new();

    [Header("Spawn Timing")]
    [SerializeField] private float respawnDelay = 8f;
    [SerializeField] private bool spawnOnStart = true;

    private GameObject currentPickupInstance;
    private bool isRespawning;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnPickup();
        }
    }

    public void SpawnPickup()
    {
        if (pickupPrefabs == null || pickupPrefabs.Count == 0)
        {
            Debug.LogWarning($"{gameObject.name}: No pickup prefabs assigned.");
            return;
        }

        if (currentPickupInstance != null) return;

        int randomIndex = Random.Range(0, pickupPrefabs.Count);
        GameObject chosenPickup = pickupPrefabs[randomIndex];

        currentPickupInstance = Instantiate(chosenPickup, transform.position, transform.rotation);
        
        SpawnedPickup spawnedPickup = currentPickupInstance.GetComponent<SpawnedPickup>();
        if (spawnedPickup == null)
        {
            spawnedPickup = currentPickupInstance.AddComponent<SpawnedPickup>();
        }

        spawnedPickup.Initialize(this);
    }

    public void NotifyPickupCollected()
    {
        currentPickupInstance = null;

        if (!isRespawning)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);
        SpawnPickup();
        isRespawning = false;
    }
}