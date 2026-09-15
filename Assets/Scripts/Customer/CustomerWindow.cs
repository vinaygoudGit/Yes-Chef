using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerWindow : MonoBehaviour, IInteractable
{
    [SerializeField] private List<IngredientData> possibleIngredients;
    [SerializeField] private float respawnDelay = 5f;

    public Order CurrentOrder { get; private set; }

    public event Action OnOrderChanged;
    public event Action<int> OnOrderCompleted;

    private void OnEnable() => GameManager.Instance.OnStateChanged += HandleGameStateChanged;
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing && CurrentOrder == null)
            SpawnNewOrder();
    }

    public void Interact(PlayerInventory inventory)
    {
        if (CurrentOrder == null || !inventory.IsHoldingSomething)
            return;

        HeldIngredient held = inventory.CurrentItem;
        if (!held.IsUsable)
            return;

        int matchIndex = FindUnfulfilledMatch(held.Data.type);
        if (matchIndex < 0)
            return; // No matching requirement - per spec, ingredient stays in hand.

        CurrentOrder.fulfilled[matchIndex] = true;
        inventory.Release();

        if (CurrentOrder.IsComplete)
        {
            CompleteOrder();
        }
        else
        {
            OnOrderChanged?.Invoke();
        }
    }

    private int FindUnfulfilledMatch(IngredientType type)
    {
        for (int i = 0; i < CurrentOrder.requiredIngredients.Count; i++)
        {
            if (!CurrentOrder.fulfilled[i] && CurrentOrder.requiredIngredients[i].type == type)
                return i;
        }
        return -1;
    }

    private void CompleteOrder()
    {
        int rawScore = 0;
        foreach (IngredientData ingredient in CurrentOrder.requiredIngredients)
            rawScore += ingredient.scoreValue;

        int secondsElapsed = Mathf.FloorToInt(Time.time - CurrentOrder.timeOpened);
        int finalScore = rawScore - secondsElapsed;

        GameEvents.RaiseOrderCompleted(finalScore);
        OnOrderCompleted?.Invoke(finalScore);

        CurrentOrder = null;
        OnOrderChanged?.Invoke();

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            SpawnNewOrder();
    }

    private void SpawnNewOrder()
    {
        int ingredientCount = UnityEngine.Random.value < 0.5f ? 2 : 3;
        var required = new List<IngredientData>(ingredientCount);

        for (int i = 0; i < ingredientCount; i++)
            required.Add(possibleIngredients[UnityEngine.Random.Range(0, possibleIngredients.Count)]);

        CurrentOrder = new Order
        {
            requiredIngredients = required,
            fulfilled = new List<bool>(new bool[ingredientCount]),
            timeOpened = Time.time
        };

        GameEvents.RaiseOrderSpawned(this, CurrentOrder);
        OnOrderChanged?.Invoke();
    }
}