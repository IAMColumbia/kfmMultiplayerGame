using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        Multiplayer,
        Exploration
    }

    public static GameManager Instance;

    [Header("Round Settings")]
    [SerializeField] private float roundLength = 180f;
    [SerializeField] private float endRoundDelay = 4f;
    [SerializeField] private AudioClip roundEndClip;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private float musicNormalVolume = 0.2f;
    [SerializeField] private float musicFadeVolume = 0.05f;
    [SerializeField] private float musicFadeTime = 1.2f;

    private float timer;
    private bool roundActive = false;
    private int lastTrackIndex = -1;
    private Coroutine musicFadeCoroutine;

    public float TimeRemaining => timer;
    public bool RoundActive => roundActive;
    public GameMode CurrentMode { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentMode = GameSession.SelectedMode;
        timer = roundLength;
    }

    private void Start()
    {
        timer = roundLength;
        roundActive = false;

        if (musicSource != null)
            musicSource.volume = musicNormalVolume;

        PlayRandomTrack();
    }

    private void Update()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            PlayRandomTrack();
        }

        if (CurrentMode != GameMode.Multiplayer)
            return;

        if (!roundActive)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            EndRound();
        }
    }

    public void SetGameMode(GameMode mode)
    {
        CurrentMode = mode;

        if (CurrentMode == GameMode.Exploration)
        {
            StartExplorationMode();
        }
        else
        {
            PauseRound();
            timer = roundLength;
        }
    }

    public void StartRound()
    {
        CurrentMode = GameMode.Multiplayer;
        GameSession.SelectedMode = GameMode.Multiplayer;

        timer = roundLength;
        roundActive = true;

        StartMusicFade(musicNormalVolume);

        PlayerIdentity[] players = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        foreach (PlayerIdentity player in players)
        {
            player.ResetScore();
            player.RespawnAndRedisguise();
            player.SetGameplayEnabled(true);
        }

        if (RoundUI.Instance != null)
            RoundUI.Instance.HideWinnerText();

        Debug.Log("Round started.");
    }

    public void StartExplorationMode()
    {
        CurrentMode = GameMode.Exploration;
        GameSession.SelectedMode = GameMode.Exploration;

        roundActive = false;
        timer = roundLength;

        StartMusicFade(musicNormalVolume);

        PlayerIdentity[] players = FindObjectsByType<PlayerIdentity>(FindObjectsSortMode.None);

        foreach (PlayerIdentity player in players)
        {
            player.SetGameplayEnabled(true);
        }

        if (RoundUI.Instance != null)
            RoundUI.Instance.HideWinnerText();

        Debug.Log("Exploration mode started.");
    }

    public void PauseRound()
    {
        roundActive = false;
    }

    private void EndRound()
    {
        if (CurrentMode != GameMode.Multiplayer)
            return;

        roundActive = false;

        StartMusicFade(musicFadeVolume);

        if (roundEndClip != null && sfxSource != null)
        {
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(roundEndClip, 2.5f);
        }

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
                RoundUI.Instance.ShowWinnerText("Tie Game!");
        }
        else
        {
            Debug.Log($"Round over! Winner: Player {winner.playerIndex} with {winner.score} points.");

            if (RoundUI.Instance != null)
                RoundUI.Instance.ShowWinnerText($"Player {winner.playerIndex + 1} Wins!");
        }

        StartCoroutine(RestartRoundRoutine());
    }

    private IEnumerator RestartRoundRoutine()
    {
        yield return new WaitForSeconds(endRoundDelay);

        if (CurrentMode == GameMode.Multiplayer)
        {
            StartRound();
            StartMusicFade(musicNormalVolume);
        }
    }

    private IEnumerator FadeMusic(float targetVolume)
    {
        if (musicSource == null)
            yield break;

        float startVolume = musicSource.volume;
        float fadeTimer = 0f;

        while (fadeTimer < musicFadeTime)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / musicFadeTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        musicSource.volume = targetVolume;
        musicFadeCoroutine = null;
    }

    private void PlayRandomTrack()
    {
        if (musicTracks == null || musicTracks.Length == 0 || musicSource == null)
            return;

        int index;

        do
        {
            index = Random.Range(0, musicTracks.Length);
        }
        while (musicTracks.Length > 1 && index == lastTrackIndex);

        lastTrackIndex = index;
        musicSource.clip = musicTracks[index];
        musicSource.Play();
    }

    private void StartMusicFade(float targetVolume)
    {
        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(FadeMusic(targetVolume));
    }
}