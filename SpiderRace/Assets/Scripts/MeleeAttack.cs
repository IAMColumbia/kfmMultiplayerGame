using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Melee Settings")]
    public int damage = 25;
    public float range = 2.0f;
    public float radius = 0.6f;
    public float attackCooldown = 0.35f;

    [Header("Hit Settings")]
    public LayerMask hitMask = ~0; // hits everything by default

    private float lastAttackTime = -999f;

    // Hook this to PlayerInput -> Events -> Attack
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;

        if (playerCamera == null)
        {
            Debug.LogError("MeleeAttack: playerCamera is not assigned.");
            return;
        }

        // SphereCast from the camera forward to find something in punching range
        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            // Don't hit yourself
            if (hit.transform.root == transform.root) return;

            if (hit.collider.TryGetComponent<Health>(out Health targetHealth))
            {
                targetHealth.TakeDamage(damage);
            }

            // Debug visual
            Debug.DrawLine(origin, hit.point, Color.yellow, 0.5f);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * range, Color.cyan, 0.5f);
        }
    }
}