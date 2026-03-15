using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerFeedbackUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI tagSuccessText;
    [SerializeField] private CanvasGroup tagSuccessCanvasGroup;

    [SerializeField] private TextMeshProUGUI tagFailText;
    [SerializeField] private CanvasGroup tagFailCanvasGroup;

    [Header("Popup Animation")]
    [SerializeField] private float popupDuration = 0.8f;
    [SerializeField] private float startScale = 0.6f;
    [SerializeField] private float peakScale = 1.15f;
    [SerializeField] private float endScale = 1f;

    private Coroutine successCoroutine;
    private Coroutine failCoroutine;

    private void Awake()
    {
        HideTagSuccessImmediate();
        HideTagFailImmediate();
    }

    public void ShowTagSuccess()
    {
        if (tagSuccessText == null || tagSuccessCanvasGroup == null) return;

        if (successCoroutine != null)
            StopCoroutine(successCoroutine);

        successCoroutine = StartCoroutine(PopupRoutine(
            tagSuccessText,
            tagSuccessCanvasGroup,
            "Tag Success!"
        ));
    }

    public void ShowTagFail()
    {
        if (tagFailText == null || tagFailCanvasGroup == null) return;

        if (failCoroutine != null)
            StopCoroutine(failCoroutine);

        failCoroutine = StartCoroutine(PopupRoutine(
            tagFailText,
            tagFailCanvasGroup,
            "Tag Fail"
        ));
    }

    private IEnumerator PopupRoutine(TextMeshProUGUI text, CanvasGroup canvasGroup, string message)
    {
        text.gameObject.SetActive(true);
        text.text = message;

        RectTransform rect = text.rectTransform;
        float elapsed = 0f;

        canvasGroup.alpha = 1f;
        rect.localScale = Vector3.one * startScale;

        while (elapsed < popupDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popupDuration;

            if (t < 0.35f)
            {
                float popT = t / 0.35f;
                float scale = Mathf.Lerp(startScale, peakScale, popT);
                rect.localScale = Vector3.one * scale;
            }
            else
            {
                float settleT = (t - 0.35f) / 0.65f;
                float scale = Mathf.Lerp(peakScale, endScale, settleT);
                rect.localScale = Vector3.one * scale;
            }

            if (t < 0.4f)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                float fadeT = (t - 0.4f) / 0.6f;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeT);
            }

            yield return null;
        }

        text.gameObject.SetActive(false);
        rect.localScale = Vector3.one * endScale;
        canvasGroup.alpha = 0f;
    }

    private void HideTagSuccessImmediate()
    {
        if (tagSuccessText == null) return;

        tagSuccessText.gameObject.SetActive(false);
        tagSuccessText.rectTransform.localScale = Vector3.one * endScale;

        if (tagSuccessCanvasGroup != null)
            tagSuccessCanvasGroup.alpha = 0f;
    }

    private void HideTagFailImmediate()
    {
        if (tagFailText == null) return;

        tagFailText.gameObject.SetActive(false);
        tagFailText.rectTransform.localScale = Vector3.one * endScale;

        if (tagFailCanvasGroup != null)
            tagFailCanvasGroup.alpha = 0f;
    }
}
