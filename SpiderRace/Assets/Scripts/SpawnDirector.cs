using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputManager inputManager;

    [Header("Player Prefabs (Order matters)")]
    [SerializeField] private GameObject rangedPlayerPrefab;
    [SerializeField] private GameObject brawlerPlayerPrefab;

    [Header("Spawn Points (size 2)")]
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;

    private int joinedCount = 0;

    private void Reset()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        if (inputManager == null)
            inputManager = GetComponent<PlayerInputManager>();

        if (inputManager == null)
        {
            Debug.LogError("SpawnDirector: No PlayerInputManager reference found.");
            enabled = false;
            return;
        }

        // Start with Player 1 prefab
        inputManager.playerPrefab = rangedPlayerPrefab;

        // Subscribe to join event
        inputManager.onPlayerJoined += HandlePlayerJoined;
    }

    private void OnDisable()
    {
        if (inputManager != null)
            inputManager.onPlayerJoined -= HandlePlayerJoined;
    }

    private void HandlePlayerJoined(PlayerInput playerInput)
    {
        joinedCount++;

        // Pick spawn by join order
        Transform spawn = joinedCount == 1 ? player1Spawn : player2Spawn;

        if (spawn != null)
        {
            playerInput.transform.SetPositionAndRotation(spawn.position, spawn.rotation);

            // Tell the Health script where this player's respawn point should be
            var health = playerInput.GetComponent<Health>();
            if (health != null)
            {
                health.SetSpawnPoint(spawn.position);
            }
        }
        else
        {
            Debug.LogWarning($"SpawnDirector: Spawn point for player {joinedCount} is not assigned.");
        }

        // Ensure PlayerInput.camera is set (needed for split-screen setup)
        Camera cam = playerInput.GetComponentInChildren<Camera>(true);
        if (cam != null)
        {
            playerInput.camera = cam;
        }
        else
        {
            Debug.LogError($"SpawnDirector: No Camera found in Player {joinedCount} prefab hierarchy.");
        }

        // AudioListener: keep only on Player 1
        AudioListener listener = cam != null ? cam.GetComponent<AudioListener>() : null;
        if (listener != null)
        {
            listener.enabled = (joinedCount == 1);
        }

        // After Player 1 joins, swap the prefab so Player 2 spawns as Brawler
        if (joinedCount == 1)
        {
            inputManager.playerPrefab = brawlerPlayerPrefab;
        }

        // Stop further joins if you only want 2 players
        if (joinedCount >= 2)
        {
            inputManager.DisableJoining();
        }
    }
}