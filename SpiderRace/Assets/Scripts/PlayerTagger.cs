using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTagger : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float tagRange = 3f;
    [SerializeField] private LayerMask tagMask = ~0;

    private PlayerInput playerInput;
    private PlayerIdentity identity;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        identity = GetComponent<PlayerIdentity>();
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
        TryTag();
    }

    private void TryTag()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, tagRange, tagMask, QueryTriggerInteraction.Ignore))
        {
            TagTarget target = hit.collider.GetComponentInParent<TagTarget>();

            if (target != null && target.Owner != identity)
            {
                identity.AddScore(1);
                target.Owner.RespawnAndRedisguise();
                return;
            }
        }

        // false tag
        identity.RespawnAndRedisguise();
    }
}