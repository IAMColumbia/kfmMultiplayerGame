using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTagger : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float tagRange = 3f;
    [SerializeField] private LayerMask tagMask = ~0;
    [SerializeField] private AudioClip tagSuccessClip;
    [SerializeField] private AudioClip tagFailClip;
    [SerializeField] private float tagRadius = 0.35f;
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
        if (playerCamera == null)
        {
            Debug.LogError("PlayerTagger has no playerCamera assigned.");
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * tagRange, Color.red, 2f);

        if (Physics.SphereCast(ray, tagRadius, out RaycastHit hit, tagRange, tagMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"Raycast hit: {hit.collider.name}");

            TagTarget target = hit.collider.GetComponentInParent<TagTarget>();

            if (target == null)
            {
                Debug.Log("Hit something, but no TagTarget was found in parent chain.");
            }
            else if (target.Owner == null)
            {
                Debug.Log("TagTarget exists, but Owner is NULL.");
            }
            else if (target.Owner == identity)
            {
                Debug.Log("You hit your own TagTarget.");
            }
            else
            {
                Debug.Log($"SUCCESS: Player {identity.playerIndex} tagged Player {target.Owner.playerIndex}");
                identity.AddScore(1);
                if (feedbackUI != null)
                {
                    feedbackUI.ShowTagSuccess();
                }
                

                if (tagSuccessClip != null && sfxSource != null)
                {
                    sfxSource.pitch = Random.Range(0.9f, 1.1f);
                    sfxSource.PlayOneShot(tagSuccessClip, 1.5f);
                }

                target.Owner.RespawnAndRedisguise();
                return;
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing.");
        }

        Debug.Log("FALSE TAG");
        if (feedbackUI != null)
        {
            feedbackUI.ShowTagFail();
        }
        if (tagFailClip != null && sfxSource != null)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(tagFailClip, 1.5f);
        }

        identity.RespawnAndRedisguise();
    }

}