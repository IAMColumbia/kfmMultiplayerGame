using TMPro;
using UnityEngine;

public class SimpleMatchUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2ScoreText;

    [Header("Match Data")]
    [SerializeField] private float matchTime = 60f;

    private float currentTime;
    private int player1Score = 0;
    private int player2Score = 0;

    private void Start()
    {
        currentTime = matchTime;
        RefreshUI();
    }

    private void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0f)
                currentTime = 0f;

            RefreshTimer();
        }
    }

    public void AddScorePlayer1()
    {
        player1Score++;
        RefreshScores();
    }

    public void AddScorePlayer2()
    {
        player2Score++;
        RefreshScores();
    }

    private void RefreshUI()
    {
        RefreshTimer();
        RefreshScores();
    }

    private void RefreshTimer()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = "Time: " + seconds;
    }

    private void RefreshScores()
    {
        player1ScoreText.text = "P1: " + player1Score;
        player2ScoreText.text = "P2: " + player2Score;
    }
}