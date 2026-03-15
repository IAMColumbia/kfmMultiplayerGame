using UnityEngine;

public class WaypointRoamer : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Transform[] waypoints;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float arriveDistance = 0.1f;
    [SerializeField] private bool loop = true;

    [Header("Pause")]
    [SerializeField] private float pauseDuration = 0.5f;

    private int currentIndex = 0;
    private float pauseTimer = 0f;

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        Transform target = waypoints[currentIndex];
        Vector3 targetPosition = target.position;

        Vector3 toTarget = targetPosition - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude <= arriveDistance * arriveDistance)
        {
            AdvanceWaypoint();
            pauseTimer = pauseDuration;
            return;
        }

        Vector3 direction = toTarget.normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void AdvanceWaypoint()
    {
        if (loop)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
        else
        {
            if (currentIndex < waypoints.Length - 1)
                currentIndex++;
        }
    }
}