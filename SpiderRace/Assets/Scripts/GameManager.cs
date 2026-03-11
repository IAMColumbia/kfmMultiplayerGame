using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float roundLength = 180f;

    private float timer;
    private bool roundActive = true;

    private void Awake()
    {
        Instance = this;
        timer = roundLength;
    }

    private void Update()
    {
        if (!roundActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        roundActive = false;

        PlayerIdentity[] players = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        PlayerIdentity winner = null;
        int bestScore = int.MinValue;

        foreach (var player in players)
        {
            if (player.score > bestScore)
            {
                bestScore = player.score;
                winner = player;
            }
        }

        if (winner != null)
            Debug.Log($"Player {winner.playerIndex} wins with {winner.score} points!");
    }
}