using UnityEngine;

public class Refrigerator : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        if (inventory.IsHoldingSomething)
            return;

        IngredientSelectionUI.Instance.Show(chosenIngredient =>
        {
            inventory.TryHold(new HeldIngredient(chosenIngredient));
        });
    }
}