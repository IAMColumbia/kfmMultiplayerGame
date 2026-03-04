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

    private bool isDead = false;

        private void OnEnable()
    {
        // When spawned/enabled, start alive
        isDead = false;
        currentHealth = maxHealth;
    }

    void Start()
    {
        spawnPoint = transform.position;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log($"{gameObject.name} died.");

        if (canRespawn)
        {
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Respawn()
    {
        transform.position = spawnPoint;
        currentHealth = maxHealth;
        isDead = false;

        gameObject.SetActive(true);

        Debug.Log($"{gameObject.name} respawned.");
    }
}