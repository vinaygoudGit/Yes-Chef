using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public HeldIngredient CurrentItem { get; private set; }
    public bool IsHoldingSomething => CurrentItem != null;


    public event Action<HeldIngredient> OnItemChanged;

    public bool TryHold(HeldIngredient item)
    {
        if (IsHoldingSomething || item == null)
            return false;

        CurrentItem = item;
        OnItemChanged?.Invoke(CurrentItem);
        return true;
    }

    public HeldIngredient Release()
    {
        HeldIngredient released = CurrentItem;
        CurrentItem = null;
        OnItemChanged?.Invoke(null);
        return released;
    }

    public void MarkCurrentAsPrepared()
    {
        CurrentItem?.MarkPrepared();
        OnItemChanged?.Invoke(CurrentItem);
    }
}