using UnityEngine;

public class ShowInteractUI : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private GameObject uiPromptPanel;

    private void Awake()
    {
        uiPromptPanel.SetActive(false);
    }

    private void OnEnable()
    {
        detector.OnTargetChanged += HandleTargetChanged;
    }

    private void OnDisable()
    {
        detector.OnTargetChanged -= HandleTargetChanged;
    }

    private void HandleTargetChanged(IInteractable target)
    {
        uiPromptPanel.SetActive(target != null);
    }
}