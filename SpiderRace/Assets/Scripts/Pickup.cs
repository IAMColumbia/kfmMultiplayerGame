using UnityEngine;

public class Pickup : MonoBehaviour
{
        public enum PickupType
    {
        ChangeAppearance,
        Invisibility,
        Teleport,
        SpeedBoost
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerups powerups = other.GetComponent<PlayerPowerups>();

        if (powerups == null)
            powerups = other.GetComponentInParent<PlayerPowerups>();

        if (powerups == null) return;

        powerups.ApplyPickup(pickupType, duration);

        SpawnedPickup spawnedPickup = GetComponent<SpawnedPickup>();
        if (spawnedPickup != null)
        {
            spawnedPickup.NotifyCollected();
        }

        Destroy(gameObject);
    }
}