using UnityEngine;

public class Bobbing : MonoBehaviour
{
    // Adjust these public variables in the Unity Inspector
    public float amplitude = 0.5f; // The maximum distance the object will move up/down from its starting point
    public float speed = 1f;       // The speed of the bobbing motion
    
    private float startY; // The initial Y position of the object

    void Start()
    {
        // Save the starting Y position
        startY = transform.localPosition.y; // Use localPosition for nested objects
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        // Time.time gives the current time in seconds since the game started
        // Mathf.Sin ranges from -1 to 1, creating a smooth oscillation
        float newY = startY + amplitude * Mathf.Sin(speed * Time.time);

        // Update the object's position
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}
