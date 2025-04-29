public interface IItemEffect
{
    ItemTriggerTiming TriggerTiming { get; }
    void Apply(BattleContext ctx);
    string GetDescription();
}


public static class ItemEffectFactory
{
    public static IItemEffect CreateEffectLogic(int itemId)
    {
        return itemId switch
        {
            1 => new Effect_ShabbyHat(),
            2 => new Effect_ScreamingClayJar(),
            3 => new Effect_MismatchedTacticalNotes(),
            4 => new Effect_DizzyOwlFigurine(),
            5 => new Effect_GoldenKey(),
            // ...
            401 => new Effect_RainbowBook(),
            _ => null,
        };
    }
}
