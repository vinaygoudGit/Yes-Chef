public class HeldIngredient
{
    public IngredientData Data { get; }
    public bool IsPrepared { get; private set; }

    public HeldIngredient(IngredientData data, bool startsPrepared = false)
    {
        Data = data;
        IsPrepared = startsPrepared;
    }

    public bool IsUsable => !Data.requiresPreparation || IsPrepared;

    public void MarkPrepared()
    {
        IsPrepared = true;
    }
}