public class Stove : TimedPrepStation
{
    protected override IngredientType AcceptedType => IngredientType.Meat;
    protected override int SlotCount => 2;
}
