using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to any TimedPrepStation (Table or Stove). Each entry in slotVisuals maps
/// 1:1 by index to the station's internal slots. One Image per slot handles both
/// the raw/prepared sprite swap AND the progress fill - Image Type must be set to
/// "Filled" in the Inspector for fillAmount to have any visual effect.
/// </summary>
public class PrepStationVisual : MonoBehaviour
{
    [System.Serializable]
    public class SlotVisual
    {
        public Image icon; // Image Type: Filled
    }

    [SerializeField] private TimedPrepStation station;
    [SerializeField] private SlotVisual[] slotVisuals;

    private void Awake()
    {
        foreach (SlotVisual visual in slotVisuals)
            visual.icon.gameObject.SetActive(false);
    }

    private void OnEnable() => station.OnSlotChanged += HandleSlotChanged;
    private void OnDisable() => station.OnSlotChanged -= HandleSlotChanged;

    private void HandleSlotChanged(int index, PrepSlotState state)
    {
        SlotVisual visual = slotVisuals[index];

        if (!state.isOccupied)
        {
            visual.icon.gameObject.SetActive(false);
            return;
        }

        visual.icon.gameObject.SetActive(true);
        visual.icon.sprite = state.isDone ? state.ingredientData.preparedSprite : state.ingredientData.rawSprite;
        visual.icon.fillAmount = state.isDone ? 1f : state.normalizedProgress;
    }
}