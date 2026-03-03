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

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveSpeed * move + Vector3.up * yVelocity;

        controller.Move(velocity * Time.deltaTime);
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
        if (controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}