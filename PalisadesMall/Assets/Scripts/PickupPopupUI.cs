using System.Collections;
using TMPro;
using UnityEngine;
public class PickupPopupUI : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private float popupDuration = 1.5f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (popupRoot != null)
            popupRoot.SetActive(false);
    }

    public void ShowPickup(string pickupName)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowPickupRoutine(pickupName));
    }

    private IEnumerator ShowPickupRoutine(string pickupName)
    {
        if (popupRoot != null)
            popupRoot.SetActive(true);

        if (popupText != null)
            popupText.text = pickupName;

        yield return new WaitForSeconds(popupDuration);

        if (popupRoot != null)
            popupRoot.SetActive(false);

        currentRoutine = null;
    }
}