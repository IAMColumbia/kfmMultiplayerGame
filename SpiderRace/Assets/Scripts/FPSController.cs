using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraPivot;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Hover")]
    [Tooltip("How high above the ground the player should hover (meters).")]
    public float hoverHeight = 0.15f;

    [Tooltip("How far down we check for ground below the player.")]
    public float groundCheckDistance = 2.5f;

    [Tooltip("How quickly we correct upward to maintain hover height.")]
    public float hoverSnapSpeed = 20f;

    [Tooltip("What layers count as ground for hovering.")]
    public LayerMask groundMask = ~0;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float yVelocity;
    private float xRotation = 0f;
    
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        var actions = playerInput.actions;

        actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        actions["Move"].canceled += ctx => moveInput = Vector2.zero;

        actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        actions["Look"].canceled += ctx => lookInput = Vector2.zero;

        actions["Jump"].performed += ctx => Jump();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 horizontal = moveSpeed * move;

        // Gravity / vertical velocity
        bool grounded = controller.isGrounded;

        if (grounded && yVelocity < 0f)
        {
            // Don't "stick" downward hard. Let hover logic manage the gap.
            yVelocity = 0f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontal + Vector3.up * yVelocity;
        controller.Move(velocity * Time.deltaTime);

        ApplyHover();
    }

    void ApplyHover()
    {
        // Raycast from slightly above the player's feet to find ground distance.
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            float currentY = transform.position.y;
            float desiredY = hit.point.y + hoverHeight;

            // Only correct upward (prevents pushing the capsule into the floor)
            float deltaY = desiredY - currentY;
            if (deltaY > 0.001f)
            {
                float step = deltaY * hoverSnapSpeed * Time.deltaTime;
                controller.Move(Vector3.up * step);

                // If we were falling but we're close to the ground, kill downward velocity
                if (yVelocity < 0f && deltaY < 0.25f)
                {
                    yVelocity = 0f;
                }
            }
        }
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Rotate body (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Jump()
    {
        // Let jumping work as normal; hover will re-establish gap when you land.
        if (controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}