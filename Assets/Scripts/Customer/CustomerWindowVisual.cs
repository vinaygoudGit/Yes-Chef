using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach alongside a CustomerWindow. Reacts to its events - never touches
/// order logic directly, only reads CurrentOrder to decide what to display.
/// </summary>
public class CustomerWindowVisual : MonoBehaviour
{
    [SerializeField] private CustomerWindow window;

    [Header("Requirement icons - size must match the max order size (3)")]
    [SerializeField] private Image[] requirementIcons;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;

    [Header("Score popup")]
    [SerializeField] private GameObject scorePopupRoot;
    [SerializeField] private CanvasGroup scorePopupGroup;
    [SerializeField] private TMP_Text scorePopupText;
    [SerializeField] private float popupHoldSeconds = 1f;
    [SerializeField] private float popupFadeSeconds = 1f;

    private void Awake()
    {
        scorePopupRoot.SetActive(false);
    }

    private void OnEnable()
    {
        window.OnOrderChanged += RefreshRequirementIcons;
        window.OnOrderCompleted += ShowScorePopup;
    }

    private void OnDisable()
    {
        window.OnOrderChanged -= RefreshRequirementIcons;
        window.OnOrderCompleted -= ShowScorePopup;
    }

    private void Update()
    {
        if (window.CurrentOrder == null)
        {
            timerText.gameObject.SetActive(false);
            return;
        }

        timerText.gameObject.SetActive(true);
        int secondsOpen = Mathf.FloorToInt(Time.time - window.CurrentOrder.timeOpened);
        timerText.text = $"{secondsOpen}s";
    }

    private void RefreshRequirementIcons()
    {
        Order order = window.CurrentOrder;

        for (int i = 0; i < requirementIcons.Length; i++)
        {
            if (order == null || i >= order.requiredIngredients.Count)
            {
                requirementIcons[i].gameObject.SetActive(false);
                continue;
            }

            requirementIcons[i].gameObject.SetActive(true);
            requirementIcons[i].sprite = order.requiredIngredients[i].preparedSprite;
            requirementIcons[i].color = order.fulfilled[i] ? new Color(1f, 1f, 1f, 0.3f) : Color.white;
        }
    }

    private void ShowScorePopup(int score)
    {
        StopAllCoroutines();
        StartCoroutine(PlayScorePopup(score));
    }

    private IEnumerator PlayScorePopup(int score)
    {
        scorePopupText.text = score >= 0 ? $"+{score}" : score.ToString();
        scorePopupGroup.alpha = 1f;
        scorePopupRoot.SetActive(true);

        yield return new WaitForSeconds(popupHoldSeconds);

        float elapsed = 0f;
        while (elapsed < popupFadeSeconds)
        {
            elapsed += Time.deltaTime;
            scorePopupGroup.alpha = 1f - (elapsed / popupFadeSeconds);
            yield return null;
        }

        scorePopupRoot.SetActive(false);
    }
}
