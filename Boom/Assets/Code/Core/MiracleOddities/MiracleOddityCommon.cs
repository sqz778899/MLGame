public class MiracleOddityData : ITooltipBuilder
{
    public int ID;
    public string Name;
    public string Desc;
    public string ImageName;
    public DropedRarity Rarity;

    public IMiracleOddityEffect EffectLogic;
    public MiracleOddityTriggerTiming TriggerTiming;

    public MiracleOddityData(int id)
    {
        MiracleOddityJson json = TrunkManager.Instance.GetMiracleOddityJson(id);
        ID = json.ID;
        Name = json.Name;
        Desc = json.Desc;
        ImageName = json.ResName;
        Rarity = json.Rarity;
        TriggerTiming = json.TriggerTiming;
        EffectLogic = MiracleOddityEffectFactory.CreateEffectLogic(ID);
    }

    public void ApplyEffect(BattleContext ctx) => EffectLogic?.Apply(ctx);
    public void ApplyEffectCash(BattleContext ctx) => EffectLogic?.ApplyCash(ctx);
    public void RemoveEffect() => EffectLogic?.RemoveEffect();
    
    public ToolTipsInfo BuildTooltip() => new(Name, 1, Desc, ToolTipsType.MiracleOddity, Rarity);
}

public enum MiracleOddityTriggerTiming
{
    OnAlltimes = 0,
    OnBattleStart = 1,
    OnBulletFire = 2,
    OnBulletHitBefore = 3,
    OnBulletHitAfter = 4,
    OnEnterRoom = 5,
    Passive = 6,
    None = 99,
}