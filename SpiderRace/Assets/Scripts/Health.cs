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

    void Start()
    {
        currentHealth = maxHealth;
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

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} died.");

        if (canRespawn)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = spawnPoint;
        isDead = false;

        gameObject.SetActive(true);

        Debug.Log($"{gameObject.name} respawned.");
    }
}