using System.Collections.Generic;

/// <summary>
/// Plain data - no MonoBehaviour needed, this never exists as its own GameObject.
/// One instance = one active order at one window.
/// </summary>
public class Order
{
    public List<IngredientData> requiredIngredients;
    public List<bool> fulfilled;
    public float timeOpened;

    public bool IsComplete => fulfilled.TrueForAll(f => f);
}
