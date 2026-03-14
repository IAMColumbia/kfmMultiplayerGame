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

    [Header("Look")]
    public float mouseSensitivity = 0.15f;
    public float gamepadLookSpeed = 180f;
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

        bool grounded = controller.isGrounded;

        if (grounded && yVelocity < 0f)
        {
            yVelocity = 0f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontal + Vector3.up * yVelocity;
        controller.Move(velocity * Time.deltaTime);

        ApplyHover();
    }

    void ApplyHover()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            float currentY = transform.position.y;
            float desiredY = hit.point.y + hoverHeight;

            float deltaY = desiredY - currentY;
            if (deltaY > 0.001f)
            {
                float step = deltaY * hoverSnapSpeed * Time.deltaTime;
                controller.Move(Vector3.up * step);

                if (yVelocity < 0f && deltaY < 0.25f)
                {
                    yVelocity = 0f;
                }
            }
        }
    }

    void HandleLook()
    {
        bool usingGamepad = Gamepad.current != null && Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.0001f;

        float lookX;
        float lookY;

        if (usingGamepad)
        {
            // Stick input is a direction/speed, so scale by turn speed and deltaTime
            lookX = lookInput.x * gamepadLookSpeed * Time.deltaTime;
            lookY = lookInput.y * gamepadLookSpeed * Time.deltaTime;
        }
        else
        {
            // Mouse input is already delta-like
            lookX = lookInput.x * mouseSensitivity;
            lookY = lookInput.y * mouseSensitivity;
        }

        transform.Rotate(Vector3.up * lookX);

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}