using UnityEngine;

public class SpawnedPickup : MonoBehaviour
{
    private PickupSpawner spawner;

    public void Initialize(PickupSpawner pickupSpawner)
    {
        spawner = pickupSpawner;
    }

    public void NotifyCollected()
    {
        if (spawner != null)
        {
            spawner.NotifyPickupCollected();
        }
    }
}
