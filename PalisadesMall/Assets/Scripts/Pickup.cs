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
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerups powerups = other.GetComponent<PlayerPowerups>();

        if (powerups == null)
            powerups = other.GetComponentInParent<PlayerPowerups>();

        if (powerups == null) return;

        powerups.ApplyPickup(pickupType, duration);
        powerups.PlayPickupSound(pickupSound, 2f);

        PlayerIdentity playerIdentity = other.GetComponent<PlayerIdentity>();

        if (playerIdentity == null)
            playerIdentity = other.GetComponentInParent<PlayerIdentity>();

        if (playerIdentity != null)
        {
            playerIdentity.ShowPickupPopup(GetPickupDisplayName());
        }

        SpawnedPickup spawnedPickup = GetComponent<SpawnedPickup>();
        if (spawnedPickup != null)
        {
            spawnedPickup.NotifyCollected();
        }

        Destroy(gameObject);
    }

    private string GetPickupDisplayName()
    {
        switch (pickupType)
        {
            case PickupType.ChangeAppearance:
                return "Appearance changed!";
            case PickupType.Invisibility:
                return "Invisibility acquired! (10 seconds)";
            case PickupType.Teleport:
                return "Player teleported!";
            case PickupType.SpeedBoost:
                return "Speed boosted! (5 seconds)";
            default:
                return "Pickup";
        }
    }
}