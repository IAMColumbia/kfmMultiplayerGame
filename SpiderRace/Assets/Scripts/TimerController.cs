using UnityEngine;
using TMPro;
public class TimerController : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 60f;
    [SerializeField] private TMP_Text timerText;

    private bool isRunning = false;

    private void Update()
    {
        if (!isRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining < 0f)
            timeRemaining = 0f;

        UpdateTimerUI();
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }
    }
}
