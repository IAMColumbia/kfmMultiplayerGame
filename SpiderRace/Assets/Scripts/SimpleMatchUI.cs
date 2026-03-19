using TMPro;
using UnityEngine;

public class SimpleMatchUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2ScoreText;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        bool explorationMode = GameManager.Instance.CurrentMode == GameManager.GameMode.Exploration;

        if (timerText != null)
            timerText.gameObject.SetActive(!explorationMode);

        if (player1ScoreText != null)
            player1ScoreText.gameObject.SetActive(!explorationMode);

        if (player2ScoreText != null)
            player2ScoreText.gameObject.SetActive(!explorationMode);

        if (explorationMode)
            return;

        UpdateTimer();
        UpdateScores();
    }

    private void UpdateTimer()
    {
        if (GameManager.Instance == null || timerText == null)
            return;

        float time = GameManager.Instance.TimeRemaining;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    private void UpdateScores()
    {
        if (player1ScoreText == null || player2ScoreText == null)
            return;

        PlayerIdentity[] players = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        int p1Score = 0;
        int p2Score = 0;

        foreach (PlayerIdentity player in players)
        {
            if (player.playerIndex == 0)
                p1Score = player.score;

            if (player.playerIndex == 1)
                p2Score = player.score;
        }

        player1ScoreText.text = "P1: " + p1Score;
        player2ScoreText.text = "P2: " + p2Score;
    }
}