using UnityEngine;

public class rotate : MonoBehaviour
{
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    public float speed = 50f;

    void Update()
    {
        Vector3 rotation = Vector3.zero;

        if (rotateX)
        {
            rotation += Vector3.right;   // X axis
        }

        if (rotateY)
        {
            rotation += Vector3.up;      // Y axis
        }

        if (rotateZ)
        {
            rotation += Vector3.forward; // Z axis
        }

        transform.Rotate(rotation * speed * Time.deltaTime, Space.Self);
    }
}