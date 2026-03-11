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

    public PickupType pickupType;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerups powerups = other.GetComponent<PlayerPowerups>();
        if (powerups == null) return;

        powerups.ApplyPickup(pickupType, duration);
        Destroy(gameObject);
    }
}
