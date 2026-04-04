using UnityEngine;

public class PlayerCameraSetup : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private PlayerIdentity identity;

    private void Awake()
    {
        identity = GetComponent<PlayerIdentity>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    private void Start()
    {
        if (playerCamera == null || identity == null) return;

        string ownPropLayerName = identity.playerIndex == 0 ? "Player1Prop" : "Player2Prop";
        int ownPropLayer = LayerMask.NameToLayer(ownPropLayerName);

        if (ownPropLayer == -1)
        {
            Debug.LogWarning($"Layer '{ownPropLayerName}' does not exist.");
            return;
        }

        playerCamera.cullingMask &= ~(1 << ownPropLayer);
    }
}
