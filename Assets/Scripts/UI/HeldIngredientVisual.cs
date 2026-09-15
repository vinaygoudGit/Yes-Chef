using UnityEngine;

/// <summary>
/// Sits on (or references) a SpriteRenderer positioned at the player's "hand".
/// Reacts to PlayerInventory's item-changed event - never spawns or destroys anything.
/// </summary>
public class HeldIngredientVisual : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private SpriteRenderer handRenderer;

    private void Awake()
    {
        handRenderer.enabled = false;
    }

    private void OnEnable() => inventory.OnItemChanged += HandleItemChanged;
    private void OnDisable() => inventory.OnItemChanged -= HandleItemChanged;

    private void HandleItemChanged(HeldIngredient item)
    {
        if (item == null)
        {
            handRenderer.enabled = false;
            return;
        }

        handRenderer.enabled = true;
        handRenderer.sprite = item.IsPrepared ? item.Data.preparedSprite : item.Data.rawSprite;
    }
}
