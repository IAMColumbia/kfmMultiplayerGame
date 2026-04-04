using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        Transform spawn = SpawnManager.Instance.GetRandomSpawnPoint();
        if (spawn == null) return;

        // Disable controller before repositioning
        if (controller != null) controller.enabled = false;

        transform.position = spawn.position;
        transform.rotation = spawn.rotation;

        if (controller != null) controller.enabled = true;
    }
}