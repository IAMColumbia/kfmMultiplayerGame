using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainMenuController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip clickSound;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    public void StartGame()
    {
        uiAudioSource.PlayOneShot(clickSound);
        StartCoroutine(FadeAndLoad());
    }

    public void QuitGame()
    {
        uiAudioSource.PlayOneShot(clickSound);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Debug.Log("Quit not supported in WebGL");
#else
        Application.Quit();
#endif
    }

    private IEnumerator FadeAndLoad()
    {
        float startVolume = musicSource.volume;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0f;

        SceneManager.LoadScene("MallGame");
    }
}
