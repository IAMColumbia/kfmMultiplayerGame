using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdentity : MonoBehaviour
{
    public int playerIndex;
    public int score;

    private PlayerInput playerInput;
    private PlayerSetup playerSetup;
    private PropDisguise propDisguise;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerSetup = GetComponent<PlayerSetup>();
        propDisguise = GetComponent<PropDisguise>();
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

    public void RespawnAndRedisguise()
    {
        playerSetup.Respawn();

        if (propDisguise != null)
            propDisguise.AssignRandomProp();
    }
}