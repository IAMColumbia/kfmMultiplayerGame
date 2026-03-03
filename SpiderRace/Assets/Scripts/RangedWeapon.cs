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
            // Don't hit yourself
            if (hit.transform.root == transform.root) return;

            if (hit.collider.TryGetComponent<Health>(out Health targetHealth))
            {
                targetHealth.TakeDamage(damage);
            }

            // Debug visual
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.5f);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * range, Color.green, 0.5f);
        }
    }
}
