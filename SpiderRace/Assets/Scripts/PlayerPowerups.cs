using System.Collections;
using UnityEngine;
public class PlayerPowerups : MonoBehaviour
{
    private PropDisguise propDisguise;
    private PlayerSetup playerSetup;
    private FPSController fpsController;
    private Renderer[] propRenderers;

    private void Awake()
    {
        propDisguise = GetComponent<PropDisguise>();
        playerSetup = GetComponent<PlayerSetup>();
        fpsController = GetComponent<FPSController>();
    }

    public void ApplyPickup(Pickup.PickupType type, float duration)
    {
        switch (type)
        {
            case Pickup.PickupType.ChangeAppearance:
                propDisguise.AssignRandomProp();
                break;

            case Pickup.PickupType.Teleport:
                playerSetup.Respawn();
                break;

            case Pickup.PickupType.SpeedBoost:
                StartCoroutine(SpeedBoostRoutine(duration));
                break;

            case Pickup.PickupType.Invisibility:
                StartCoroutine(InvisibilityRoutine(duration));
                break;
        }
    }

    private IEnumerator SpeedBoostRoutine(float duration)
    {
        float originalSpeed = fpsController.moveSpeed;
        fpsController.moveSpeed *= 1.5f;

        yield return new WaitForSeconds(duration);

        fpsController.moveSpeed = originalSpeed;
    }

    private IEnumerator InvisibilityRoutine(float duration)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (var r in renderers)
            r.enabled = false;

        yield return new WaitForSeconds(duration);

        foreach (var r in renderers)
            r.enabled = true;
    }
}