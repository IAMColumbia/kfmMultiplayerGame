using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTagger : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float tagRange = 20f;
    [SerializeField] private AudioClip tagSuccessClip;
    [SerializeField] private AudioClip tagFailClip;
    [SerializeField] private AudioSource sfxSource;

    private PlayerInput playerInput;
    private PlayerIdentity identity;
    private PlayerFeedbackUI feedbackUI;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        identity = GetComponent<PlayerIdentity>();
        feedbackUI = GetComponentInChildren<PlayerFeedbackUI>();
    }

    private void OnEnable()
    {
        var actions = playerInput.actions;
        actions["Tag"].performed += OnTagPerformed;
    }

    private void OnDisable()
    {
        if (playerInput == null) return;

        var actions = playerInput.actions;
        actions["Tag"].performed -= OnTagPerformed;
    }

    private void OnTagPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log($"Player {identity.playerIndex} pressed TAG");
        TryTag();
    }

    private void TryTag()
    {
        PlayerIdentity bestTarget = FindBestTagTarget();

        if (bestTarget != null)
        {
            Debug.Log($"SUCCESS: Player {identity.playerIndex} tagged Player {bestTarget.playerIndex}");

            identity.AddScore(1);

            if (feedbackUI != null)
            {
                feedbackUI.ShowTagSuccess();
            }

            if (tagSuccessClip != null && sfxSource != null)
            {
                sfxSource.pitch = Random.Range(0.9f, 1.1f);
                sfxSource.PlayOneShot(tagSuccessClip, 2f);
            }

            bestTarget.RespawnAndRedisguise();
            return;
        }

        Debug.Log("FALSE TAG");

        // 🚫 In exploration mode, ignore failed tags completely
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentMode == GameManager.GameMode.Exploration)
        {
            return;
        }

        if (feedbackUI != null)
        {
            feedbackUI.ShowTagFail();
        }

        if (tagFailClip != null && sfxSource != null)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(tagFailClip, 2f);
        }

    }

    private PlayerIdentity FindBestTagTarget()
    {
        PlayerIdentity[] allPlayers = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        PlayerIdentity bestTarget = null;
        float bestDistance = float.MaxValue;

        Vector3 myPosition = transform.position;
        Vector2 myFlat = new Vector2(myPosition.x, myPosition.z);

        foreach (PlayerIdentity otherPlayer in allPlayers)
        {
            if (otherPlayer == null || otherPlayer == identity)
                continue;

            Vector3 otherPosition = otherPlayer.transform.position;
            Vector2 otherFlat = new Vector2(otherPosition.x, otherPosition.z);

            float distance = Vector2.Distance(myFlat, otherFlat);

            Debug.Log($"Flat distance from Player {identity.playerIndex} to Player {otherPlayer.playerIndex}: {distance}");

            if (distance > tagRange)
                continue;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTarget = otherPlayer;
            }
        }

        return bestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tagRange);
    }
}