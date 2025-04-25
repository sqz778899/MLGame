public class Buff_SecondBulletNerf : IBuffEffect
{
    public string BuffId => "SecondBulletNerf";
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;

    public int Stack { get; set; } = 1;
    public bool IsDebuff => true;

    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
        {
            ctx.AllBullets[1].FinalDamage -= Stack;
        }
    }

    public int RemainingBattles { get; set; } = 1;

    public bool IsExpired() => RemainingBattles <= 0; //持续几轮战斗则移除
    public string GetDescription() => $"你的第二颗子弹伤害 -{Stack}";
}

public class Buff_StrengthUp : IBuffEffect
{
    public string BuffId => "StrengthUp";
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;

    public int Stack { get; set; } = 1;
    public bool IsDebuff => false;

    public void Apply(BattleContext ctx)
    {
        foreach (var bullet in ctx.AllBullets)
        {
            bullet.FinalDamage += Stack;
        }
    }

    public int RemainingBattles { get; set; } = 1;
    
    public bool IsExpired() => RemainingBattles <= 0; //持续几轮战斗则移除
    public string GetDescription() => $"所有子弹伤害 +{Stack}";
}
