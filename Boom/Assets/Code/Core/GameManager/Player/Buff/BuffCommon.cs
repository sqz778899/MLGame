public interface IBuffEffect
{
    string BuffId { get; } // 唯一标识
    ItemTriggerTiming TriggerTiming { get; }

    int Stack { get; set; } // 当前层数
    bool IsDebuff { get; }

    void Apply(BattleContext ctx);
    int RemainingBattles { get; set; } // 剩余战斗次数
    bool IsExpired(); // 是否应移除
    string GetDescription();
}

/*public static class BuffFactory
{
    public static IBuffEffect Create(string id)
    {
        return id switch
        {
            "SecondBulletNerf" => new Buff_SecondBulletNerf(),
            "StrengthUp" => new Buff_StrengthUp(),
            _ => null,
        };
    }
}*/
