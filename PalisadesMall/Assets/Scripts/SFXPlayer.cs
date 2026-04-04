using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance;

    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip);
    }
}