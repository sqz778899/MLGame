public class MiracleOddityData : ITooltipBuilder
{
    public int ID;
    public string Name;
    public string Desc;
    public string Flavor;
    public DropedRarity Rarity;

    public IMiracleOddityEffect EffectLogic;
    public MiracleOddityTriggerTiming TriggerTiming;
    
    MiracleOddityJson _json => TrunkManager.Instance.GetMiracleOddityJson(ID);
    public MiracleOddityData(int id)
    {
        MiracleOddityJson json = TrunkManager.Instance.GetMiracleOddityJson(id);
        ID = json.ID;
        Rarity = json.Rarity;
        TriggerTiming = json.TriggerTiming;
        EffectLogic = MiracleOddityEffectFactory.CreateEffectLogic(ID);
        
        Loc.OnLanguageChanged -= SyncStrInfo;
        Loc.OnLanguageChanged += SyncStrInfo;//语言改变事件
        SyncStrInfo(json);//同步字符串信息
    }

    #region 处理本地化多语言相关
    void SyncStrInfo() => SyncStrInfo(_json);

    void SyncStrInfo(MiracleOddityJson json)
    {
        Name = Loc.Get(json.NameKey);
        Desc = Loc.Get(json.DescKey);
        Flavor = Loc.Get(json.FlavorKey);
    }
    ~MiracleOddityData() => ClearData();
    public void ClearData() => Loc.OnLanguageChanged -= SyncStrInfo;
    #endregion

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