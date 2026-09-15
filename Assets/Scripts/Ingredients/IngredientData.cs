using UnityEngine;

public enum IngredientType
{
    Vegetable,
    Cheese,
    Meat
}

[CreateAssetMenu(fileName = "NewIngredient", menuName = "YesChef/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public IngredientType type;
    public int scoreValue;
    public bool requiresPreparation;
    public Sprite rawSprite;
    public Sprite preparedSprite;
}
