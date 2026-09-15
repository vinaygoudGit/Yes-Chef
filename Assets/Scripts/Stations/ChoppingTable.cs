public class ChoppingTable : TimedPrepStation
{
    protected override IngredientType AcceptedType => IngredientType.Vegetable;
    protected override int SlotCount => 1;
}
