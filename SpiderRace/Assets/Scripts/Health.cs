using System.Collections;
using UnityEngine;
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Respawn Settings (Optional)")]
    public bool canRespawn = false;
    public float respawnDelay = 3f;

    private Vector3 spawnPoint;
    private bool hasSpawnPoint = false;

    private bool isDead = false;
    private Coroutine respawnRoutine;

    private void OnEnable()
    {
        // When spawned/enabled, start alive
        isDead = false;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Call this from your SpawnDirector after the player is placed.
    /// </summary>
    public void SetSpawnPoint(Vector3 pos)
    {
        spawnPoint = pos;
        hasSpawnPoint = true;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

private void Die()
{
    if (isDead) return;

    isDead = true;
    Debug.Log($"{gameObject.name} died.");

    if (canRespawn)
    {
        StartCoroutine(RespawnAfterDelay());
    }
    else
    {
        SetAliveState(false);
    }
}

private IEnumerator RespawnAfterDelay()
{
    SetAliveState(false);
    yield return new WaitForSeconds(respawnDelay);

    if (!hasSpawnPoint)
        spawnPoint = transform.position; // fallback

    transform.position = spawnPoint;
    currentHealth = maxHealth;
    isDead = false;

    SetAliveState(true);
    Debug.Log($"{gameObject.name} respawned.");
}

private void SetAliveState(bool alive)
{
    // Disable controls
    var cc = GetComponent<CharacterController>();
    if (cc) cc.enabled = alive;

    var pi = GetComponent<UnityEngine.InputSystem.PlayerInput>();
    if (pi) pi.enabled = alive;

    // Hide/show visuals (optional)
    foreach (var r in GetComponentsInChildren<Renderer>(true))
        r.enabled = alive;
}
    public void RespawnNow()
    {
        if (!hasSpawnPoint)
        {
            Debug.LogWarning($"{gameObject.name} has no spawnPoint set yet. Respawn skipped.");
            return;
        }

        transform.SetPositionAndRotation(spawnPoint, transform.rotation);
        currentHealth = maxHealth;
        isDead = false;

        gameObject.SetActive(true);
        Debug.Log($"{gameObject.name} respawned.");
    }
}