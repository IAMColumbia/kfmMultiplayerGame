using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class GameStarter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject waitingForPlayersPanel;
    [SerializeField] private TMP_Text joinedPlayersText;

    [Header("Player Joining")]
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private int requiredPlayers = 2;

    [Header("Gameplay")]
    [SerializeField] private MonoBehaviour[] gameplayScriptsToEnable;

    private int joinedPlayers = 0;
    private bool gameStarted = false;

    private void Start()
    {
        joinedPlayers = 0;
        gameStarted = false;

        if (waitingForPlayersPanel != null)
            waitingForPlayersPanel.SetActive(true);

        RefreshJoinedPlayersText();

        SetGameplayEnabled(false);

        if (playerInputManager != null)
            playerInputManager.EnableJoining();

        if (GameManager.Instance != null)
            GameManager.Instance.PauseRound();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (gameStarted)
            return;

        joinedPlayers++;
        RefreshJoinedPlayersText();

        Debug.Log("Player joined. Total players: " + joinedPlayers);

        if (joinedPlayers >= requiredPlayers)
        {
            BeginGame();
        }
    }

    private void BeginGame()
    {
        gameStarted = true;

        if (waitingForPlayersPanel != null)
            waitingForPlayersPanel.SetActive(false);

        if (playerInputManager != null)
            playerInputManager.DisableJoining();

        SetGameplayEnabled(true);

        if (GameManager.Instance != null)
            GameManager.Instance.StartRound();

        Debug.Log("Game started!");
    }

    private void SetGameplayEnabled(bool enabled)
    {
        if (gameplayScriptsToEnable == null)
            return;

        foreach (MonoBehaviour script in gameplayScriptsToEnable)
        {
            if (script != null)
                script.enabled = enabled;
        }
    }

    private void RefreshJoinedPlayersText()
    {
        if (joinedPlayersText != null)
        {
            joinedPlayersText.text = "Players Joined: " + joinedPlayers + " / " + requiredPlayers;
        }
    }
}