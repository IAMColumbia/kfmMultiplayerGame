using System.Collections;
using UnityEngine;
public class PlayerPowerups : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 1.5f;

    private FPSController fpsController;
    private PlayerSetup playerSetup;
    private PropDisguise propDisguise;

    private Coroutine speedRoutine;

    private void Awake()
    {
        fpsController = GetComponent<FPSController>();
        playerSetup = GetComponent<PlayerSetup>();
        propDisguise = GetComponent<PropDisguise>();
    }

    public void ApplyPickup(Pickup.PickupType type, float duration)
    {
        switch (type)
        {
            case Pickup.PickupType.SpeedBoost:
                ApplySpeedBoost(duration);
                break;

            case Pickup.PickupType.Teleport:
                ApplyTeleport();
                break;

            case Pickup.PickupType.ChangeAppearance:
                ApplyChangeAppearance();
                break;

            case Pickup.PickupType.Invisibility:
                ApplyInvisibility(duration);
                break;
        }
    }

    private void ApplySpeedBoost(float duration)
    {
        if (fpsController == null) return;

        if (speedRoutine != null)
            StopCoroutine(speedRoutine);

        speedRoutine = StartCoroutine(SpeedBoostRoutine(duration));
    }

    private IEnumerator SpeedBoostRoutine(float duration)
    {
        float originalSpeed = fpsController.moveSpeed;
        fpsController.moveSpeed = originalSpeed * speedMultiplier;

        yield return new WaitForSeconds(duration);

        fpsController.moveSpeed = originalSpeed;
        speedRoutine = null;
    }

    private void ApplyTeleport()
    {
        if (playerSetup != null)
            playerSetup.Respawn();
    }

    private void ApplyChangeAppearance()
    {
        if (propDisguise != null)
            propDisguise.AssignRandomProp();
    }

    private void ApplyInvisibility(float duration)
    {
        StartCoroutine(InvisibilityRoutine(duration));
    }

    private IEnumerator InvisibilityRoutine(float duration)
    {
        if (propDisguise == null) yield break;

        GameObject currentProp = propDisguise.GetCurrentPropInstance();
        if (currentProp == null) yield break;

        Renderer[] renderers = currentProp.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer r in renderers)
            r.enabled = false;

        yield return new WaitForSeconds(duration);

        foreach (Renderer r in renderers)
            if (r != null) r.enabled = true;
    }
}