using UnityEngine;


public interface IBuffEffect
{
    int Id { get; } // 唯一标识
    BuffData Data { get; } // Buff数据
    
    MiracleOddityTriggerTiming TriggerTiming { get; }

    int Stack { get; set; } // 当前层数
    bool IsDebuff { get; }

    void Apply(BattleContext ctx);
    int RemainingBattles { get; set; } // 剩余战斗次数
    bool IsExpired(); // 是否应移除
    string GetDescription();
}

public class BuffData:ITooltipBuilder
{
    public int ID;
    public string Name;
    public string Desc;
    public bool IsDebuff;
    public DropedRarity Rarity;
    public Sprite Icon;
    
    public BuffData(int _id)
    {
        ID = _id;
        BuffJson curJson = TrunkManager.Instance.GetBuffJson(_id);
        Name = curJson.Name;
        Desc = curJson.Desc;
        Rarity = curJson.Rarity;
        IsDebuff = curJson.IsDebuff;
        Icon = ResManager.instance.GetBuffIcon(_id);
    }

    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name, 0, Desc, ToolTipsType.Buff, Rarity);
        return info;
    }
}