using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BumperCarController : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 22f;
    public float maxSpeed = 11f;
    public float turnSpeed = 150f;
    public float reverseMultiplier = 0.7f;

    [Header("Optional")]
    public float boostMultiplier = 0.7f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool boostHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // PlayerInput (Send Messages) will call these automatically if action names match:
    void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    void OnBoost(InputValue value) => boostHeld = value.isPressed;

    void FixedUpdate()
    {
        // moveInput.y = forward/back, moveInput.x = turn
        float throttle = moveInput.y;
        float turn = moveInput.x;

        float accel = acceleration * (boostHeld ? boostMultiplier : 1f);
        if (throttle < 0f) accel *= reverseMultiplier;

        rb.AddForce(transform.forward * (throttle * accel), ForceMode.Acceleration);

        // Clamp speed on XZ
        Vector3 vel = rb.linearVelocity;
        Vector3 flat = new Vector3(vel.x, 0f, vel.z);
        if (flat.magnitude > maxSpeed * (boostHeld ? boostMultiplier : 1f))
        {
            Vector3 clamped = flat.normalized * (maxSpeed * (boostHeld ? boostMultiplier : 1f));
            rb.linearVelocity = new Vector3(clamped.x, vel.y, clamped.z);
        }

        // Turning scaled by speed so it doesn’t spin-in-place (feels bumper-carry)    }
        float speedFactor = Mathf.Clamp01(flat.magnitude / (maxSpeed * 0.4f));
        float turnAmount = turn * turnSpeed * speedFactor * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
    }
}
