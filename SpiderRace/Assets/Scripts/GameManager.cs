using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Round Settings")]
    [SerializeField] private float roundLength = 180f;
    [SerializeField] private float endRoundDelay = 4f;
    [SerializeField] private AudioClip roundEndClip;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private float normalMusicVolume = 0.2f;
    [SerializeField] private float duckedMusicVolume = 0.05f;
    [SerializeField] private float musicDuckDuration = 1.5f;
    [SerializeField] private float musicNormalVolume = 0.2f;
    [SerializeField] private float musicFadeVolume = 0.05f;
    [SerializeField] private float musicFadeTime = 1.2f;

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
        
        if (musicSource != null)
        {
            musicSource.volume = normalMusicVolume;
        }
        
        PlayRandomTrack();
    }

    private void Update()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            PlayRandomTrack();
        }

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

        StartCoroutine(FadeMusic(musicNormalVolume));

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

        StartCoroutine(FadeMusic(musicFadeVolume));

        if (musicSource != null)
        {
            musicSource.volume = duckedMusicVolume;
        }

        if (roundEndClip != null && sfxSource != null)
        {
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(roundEndClip, 1.5f);
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
        float duckTime = Mathf.Min(musicDuckDuration, endRoundDelay);

        yield return new WaitForSeconds(duckTime);

        if (musicSource != null)
        {
            musicSource.volume = normalMusicVolume;
        }

        yield return new WaitForSeconds(endRoundDelay - duckTime);

        StartRound();
    }
        private IEnumerator FadeMusic(float targetVolume)
    {
        if (musicSource == null) yield break;

        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < musicFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / musicFadeTime;

            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    
    private void PlayRandomTrack()
    {
        if (musicTracks == null || musicTracks.Length == 0)
            return;

        int index = Random.Range(0, musicTracks.Length);

        musicSource.clip = musicTracks[index];
        musicSource.Play();
    }
}