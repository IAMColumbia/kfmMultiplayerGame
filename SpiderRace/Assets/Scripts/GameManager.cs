using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Round Settings")]
    [SerializeField] private float roundLength = 180f;
    [SerializeField] private float endRoundDelay = 4f;

    private float timer;
    private bool roundActive = true;

    public float TimeRemaining => timer;
    public bool RoundActive => roundActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        timer = roundLength;
    }

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (!roundActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            EndRound();
        }
    }

    private void StartRound()
    {
        timer = roundLength;
        roundActive = true;

        PlayerIdentity[] players = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        foreach (PlayerIdentity player in players)
        {
            player.ResetScore();
            player.RespawnAndRedisguise();
            player.SetGameplayEnabled(true);
        }

        if (RoundUI.Instance != null)
        {
            RoundUI.Instance.HideWinnerText();
        }

        Debug.Log("Round started.");
    }

    private void EndRound()
    {
        roundActive = false;

        PlayerIdentity[] players = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        PlayerIdentity winner = null;
        int bestScore = int.MinValue;
        bool tie = false;

        foreach (PlayerIdentity player in players)
        {
            player.SetGameplayEnabled(false);

            if (player.score > bestScore)
            {
                bestScore = player.score;
                winner = player;
                tie = false;
            }
            else if (player.score == bestScore)
            {
                tie = true;
            }
        }

        if (tie || winner == null)
        {
            Debug.Log("Round over! It's a tie.");

            if (RoundUI.Instance != null)
            {
                RoundUI.Instance.ShowWinnerText("Tie Game!");
            }
        }
        else
        {
            Debug.Log($"Round over! Winner: Player {winner.playerIndex} with {winner.score} points.");

            if (RoundUI.Instance != null)
            {
                RoundUI.Instance.ShowWinnerText($"Player {winner.playerIndex + 1} Wins!");
            }
        }

        StartCoroutine(RestartRoundRoutine());
    }

    private IEnumerator RestartRoundRoutine()
    {
        yield return new WaitForSeconds(endRoundDelay);
        StartRound();
    }
}