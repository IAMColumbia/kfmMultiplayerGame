using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdentity : MonoBehaviour
{
    public int playerIndex;
    public int score;
    [SerializeField] private PickupPopupUI pickupPopupUI;

    private PlayerInput playerInput;
    private PlayerSetup playerSetup;
    private PropDisguise propDisguise;
    private FPSController fpsController;
    private PlayerTagger playerTagger;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerSetup = GetComponent<PlayerSetup>();
        propDisguise = GetComponent<PropDisguise>();
        fpsController = GetComponent<FPSController>();
        playerTagger = GetComponent<PlayerTagger>();
    }

    private void Start()
    {
        if (playerInput != null)
            playerIndex = playerInput.playerIndex;

        TagTarget[] allTargets = GetComponentsInChildren<TagTarget>(true);

        foreach (TagTarget target in allTargets)
        {
            target.Initialize(this);
        }
    }
    public void ShowPickupPopup(string pickupName)
    {
        if (pickupPopupUI != null)
        {
            pickupPopupUI.ShowPickup(pickupName);
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Player {playerIndex} score: {score}");
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void RespawnAndRedisguise()
    {
        if (playerSetup != null)
            playerSetup.Respawn();

        if (propDisguise != null)
            propDisguise.AssignRandomProp();
    }

    public void SetGameplayEnabled(bool enabled)
    {
        if (fpsController != null)
            fpsController.enabled = enabled;

        if (playerTagger != null)
            playerTagger.enabled = enabled;
    }
}