using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        if (inventory.IsHoldingSomething)
            inventory.Release();
    }
}