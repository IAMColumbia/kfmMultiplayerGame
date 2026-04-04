using UnityEngine;

public class MoonMove : MonoBehaviour
{
    [SerializeField] private float speed = 10f;          // horizontal movement
    [SerializeField] private float arcAmount = 2f;       // very subtle vertical movement
    [SerializeField] private float arcSpeed = 0.2f;      // slow arc

    private Vector3 startPos;
    private float time;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        time += Time.deltaTime;

        float x = startPos.x + speed * time;
        float y = startPos.y + Mathf.Sin(time * arcSpeed) * arcAmount;

        transform.position = new Vector3(x, y, startPos.z);
        
        if (transform.position.x > 1300f)
        {
            transform.position = new Vector3(-950f, startPos.y, startPos.z);
            time = 0f;
        }
    }
}