using UnityEngine;

public class Effect_RainbowBook : IItemEffect
{
    public ItemTriggerTiming TriggerTiming { get; }

    public void Apply(BattleContext ctx)
    {
    }

    public string GetDescription()
    {
        return "";
    }
}