using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdentity : MonoBehaviour
{
    public int playerIndex;
    public int score;

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