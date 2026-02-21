using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SpawnOnJoin : MonoBehaviour
{
    public Transform[] spawnPoints;

    private int nextSpawnIndex = 0;

    public void OnPlayerJoined(PlayerInput player)
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        Transform sp = spawnPoints[nextSpawnIndex % spawnPoints.Length];
        nextSpawnIndex++;

        player.transform.position = sp.position;
        player.transform.rotation = sp.rotation;
    }
}
