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

    [Header("Jump Feel")]
    public float coyoteTime = 0.12f;
    
    [Header("Look")]
    private float coyoteTimer;
    public float mouseSensitivity = 0.02f;
    public float gamepadLookSpeed = 240f;
    public float maxLookAngle = 80f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float yVelocity;
    private float xRotation = 0f;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
    }

    void OnEnable()
    {
        jumpAction.performed += OnJumpPerformed;
    }

    void OnDisable()
    {
        jumpAction.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        Jump();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();

        HandleMovement();
        HandleLook();
    }

    private bool IsGroundedByHover()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, groundCheckDistance, groundMask, QueryTriggerInteraction.Ignore))
        {
            float desiredY = hit.point.y + hoverHeight;
            float deltaY = desiredY - transform.position.y;

            // Close enough to our hover height to count as grounded
            return deltaY > -0.08f && deltaY < 0.25f;
        }

        return false;
    }

    void HandleMovement()
    {
        bool grounded = controller.isGrounded || IsGroundedByHover();

        if (grounded)
        {
            coyoteTimer = coyoteTime;

            if (yVelocity < 0f)
            {
                yVelocity = -2f;
            }
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 horizontal = move * moveSpeed;
        Vector3 velocity = horizontal + Vector3.up * yVelocity;

        controller.Move(velocity * Time.deltaTime);

        ApplyHover();
    }

    void ApplyHover()
    {
        if (yVelocity > 0f)
            return;
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
        bool usingGamepad = playerInput.currentControlScheme == "Gamepad";

        float lookX;
        float lookY;

        if (usingGamepad)
        {
            lookX = lookInput.x * gamepadLookSpeed * Time.deltaTime;
            lookY = lookInput.y * gamepadLookSpeed * Time.deltaTime;
        }
        else
        {
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
        if (coyoteTimer > 0f)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteTimer = 0f;
        }
    }
}