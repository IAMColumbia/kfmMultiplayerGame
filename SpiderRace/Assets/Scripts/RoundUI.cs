using TMPro;
using UnityEngine;
public class RoundUI : MonoBehaviour
{
    public static RoundUI Instance;

    [SerializeField] private TextMeshProUGUI winnerText;

    private void Awake()
    {
        Instance = this;
        HideWinnerText();
    }

    public void ShowWinnerText(string message)
    {
        if (winnerText == null) return;

        winnerText.gameObject.SetActive(true);
        winnerText.text = message;
    }

    public void HideWinnerText()
    {
        if (winnerText == null) return;

        winnerText.gameObject.SetActive(false);
        winnerText.text = "";
    }
}