using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Weapon Settings")]
    public int damage = 20;
    public float range = 60f;
    public float fireCooldown = 0.15f;

    [Header("Hit Settings")]
    public LayerMask hitMask = ~0; // hits everything by default

    private float lastFireTime = -999f;

    // Hook this to PlayerInput -> Events -> Attack
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"Ranged OnAttack fired: performed={ctx.performed} by {gameObject.name}");

        if (!ctx.performed) return;

        if (Time.time < lastFireTime + fireCooldown) return;
        lastFireTime = Time.time;

        if (playerCamera == null)
        {
            Debug.LogError("RangedWeapon: playerCamera is not assigned.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
        Debug.Log($"Ranged hit: {hit.collider.name} | root: {hit.transform.root.name}");

        var myPlayer = GetComponentInParent<PlayerInput>();
        var hitPlayer = hit.collider.GetComponentInParent<PlayerInput>();

        if (hitPlayer != null && myPlayer != null && hitPlayer == myPlayer)
        {
            Debug.Log("Blocked: hit self player");
            return;
        }

        Health targetHealth = hit.collider.GetComponentInParent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning($"No Health found up parent chain from {hit.collider.name}");
        }

            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.5f);
        }
    }
}
