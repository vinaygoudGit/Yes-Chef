using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A single shared selection panel used by every Refrigerator in the scene.
/// Refrigerator asks this to show itself and hands over a callback -
/// this class doesn't know or care what happens after a button is clicked.
/// </summary>
public class IngredientSelectionUI : MonoBehaviour
{
    public static IngredientSelectionUI Instance { get; private set; }

    [SerializeField] private InteractionDetector detector;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button vegetableButton;
    [SerializeField] private Button cheeseButton;
    [SerializeField] private Button meatButton;

    [SerializeField] private IngredientData vegetableData;
    [SerializeField] private IngredientData cheeseData;
    [SerializeField] private IngredientData meatData;

    public bool IsOpen => panel.activeSelf;

    private Action<IngredientData> onSelected;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);

        vegetableButton.onClick.AddListener(() => Select(vegetableData));
        cheeseButton.onClick.AddListener(() => Select(cheeseData));
        meatButton.onClick.AddListener(() => Select(meatData));
    }

    private void OnEnable()
    {
        detector.OnTargetChanged += HandleTargetChanged;
    }

    private void OnDisable()
    {
        detector.OnTargetChanged -= HandleTargetChanged;
    }

    public void Show(Action<IngredientData> onIngredientChosen)
    {
        onSelected = onIngredientChosen;
        panel.SetActive(true);
    }

    private void Select(IngredientData data)
    {
        panel.SetActive(false);
        onSelected?.Invoke(data);
        onSelected = null;
    }

    private void HandleTargetChanged(IInteractable newTarget)
    {
        // Player left the fridge's range (or moved to a different interactable) while choosing.
        if (!IsOpen)
            return;

        panel.SetActive(false);
        onSelected = null;
    }
}