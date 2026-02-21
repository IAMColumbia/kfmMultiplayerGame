using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;

    [Header("Sensitivity")]
    public float mouseSensitivity = 0.12f;
    public float stickSensitivity = 120f;

    [Header("Pitch Clamp")]
    public float minPitch = -60f;
    public float maxPitch = 60f;

    private Vector2 lookInput;
    private float pitch;

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (cameraPivot == null) return;

        bool looksLikeMouse = lookInput.sqrMagnitude > 2f;

        float yawDelta;
        float pitchDelta;

        if (looksLikeMouse)
        {
            yawDelta = lookInput.x * mouseSensitivity;
            pitchDelta = lookInput.y * mouseSensitivity;
        }
        else
        {
            yawDelta = lookInput.x * stickSensitivity * Time.deltaTime;
            pitchDelta = lookInput.y * stickSensitivity * Time.deltaTime;
        }

        transform.Rotate(0f, yawDelta, 0f);

        pitch = Mathf.Clamp(pitch - pitchDelta, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
