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
            1 => new Effect_CrazyHat(),
            7 => new Effect_LuckyBoots(),
            // ...
            401 => new Effect_RainbowBook(),
            _ => null,
        };
    }
}
