using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedPrepStation : MonoBehaviour, IInteractable
{
    protected class PrepSlot
    {
        public HeldIngredient ingredient;
        public float timeRemaining;

        public bool IsOccupied => ingredient != null;
        public bool IsDone => IsOccupied && timeRemaining <= 0f;
    }

    [SerializeField] protected float prepDuration;

    protected abstract IngredientType AcceptedType { get; }
    protected abstract int SlotCount { get; }

    protected List<PrepSlot> slots;

    /// <summary>Fired when a slot's state changes (item placed, progress ticks, item removed). UI scripts subscribe to this.</summary>
    public event Action<int, PrepSlotState> OnSlotChanged;

    protected virtual void Awake()
    {
        slots = new List<PrepSlot>(SlotCount);
        for (int i = 0; i < SlotCount; i++)
            slots.Add(new PrepSlot());
    }

    public void Interact(PlayerInventory inventory)
    {
        if (TryTakeFinishedItem(inventory))
            return;

        TryPlaceItem(inventory);
    }

    private bool TryTakeFinishedItem(PlayerInventory inventory)
    {
        if (inventory.IsHoldingSomething)
            return false;

        int slotIndex = slots.FindIndex(s => s.IsDone);
        if (slotIndex < 0)
            return false;

        PrepSlot slot = slots[slotIndex];
        inventory.TryHold(slot.ingredient);

        slot.ingredient = null;
        slot.timeRemaining = 0f;
        NotifySlotChanged(slotIndex);
        return true;
    }

    private bool TryPlaceItem(PlayerInventory inventory)
    {
        if (!inventory.IsHoldingSomething)
            return false;

        HeldIngredient held = inventory.CurrentItem;
        if (held.Data.type != AcceptedType || held.IsPrepared)
            return false;

        int slotIndex = slots.FindIndex(s => !s.IsOccupied);
        if (slotIndex < 0)
            return false;

        PrepSlot slot = slots[slotIndex];
        slot.ingredient = held;
        slot.timeRemaining = prepDuration;
        inventory.Release();

        NotifySlotChanged(slotIndex);
        return true;
    }

    protected virtual void Update()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            PrepSlot slot = slots[i];
            if (!slot.IsOccupied || slot.IsDone)
                continue;

            slot.timeRemaining -= Time.deltaTime;

            if (slot.timeRemaining <= 0f)
            {
                slot.timeRemaining = 0f;
                slot.ingredient.MarkPrepared();
            }

            NotifySlotChanged(i);
        }
    }

    private void NotifySlotChanged(int index)
    {
        PrepSlot slot = slots[index];
        var state = new PrepSlotState
        {
            isOccupied = slot.IsOccupied,
            isDone = slot.IsDone,
            normalizedProgress = slot.IsOccupied ? 1f - (slot.timeRemaining / prepDuration) : 0f,
            ingredientData = slot.IsOccupied ? slot.ingredient.Data : null
        };
        OnSlotChanged?.Invoke(index, state);
    }
}

public struct PrepSlotState
{
    public bool isOccupied;
    public bool isDone;
    public float normalizedProgress; // 0 = just placed, 1 = finished
    public IngredientData ingredientData; // null when the slot is empty
}