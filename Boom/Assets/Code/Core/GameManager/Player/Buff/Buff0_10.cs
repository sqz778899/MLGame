public class Buff_ApplyBewilderHex : IBuffEffect
{
    #region 重要信息
    public int Id => 1; // 固定写死，对应表里ID
    public string Name => "迷乱咒印"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_ApplyBewilderHex() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"开火时子弹顺序打乱，持续一场战斗";
    #endregion
}

public class Buff_LingeringManaTaint : IBuffEffect
{
    #region 重要信息
    public int Id => 2; // 固定写死，对应表里ID
    public string Name => "魔力残毒"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_LingeringManaTaint() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"第一颗子弹宝石效果-1，持续一场战斗";
    #endregion
}

public class Buff_CurseExhaustion : IBuffEffect
{
    #region 重要信息
    public int Id => 3; // 固定写死，对应表里ID
    public string Name => "咒术疲劳"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_CurseExhaustion() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"第二颗子弹伤害-1，持续一场战斗";
    #endregion
}

public class Buff_ArcaneOverload : IBuffEffect
{
    #region 重要信息
    public int Id => 4; // 固定写死，对应表里ID
    public string Name => "魔力短路"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_ArcaneOverload() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"第一颗子弹穿透失效，持续一场战斗";
    #endregion
}

public class Buff_AeverseManaSurge : IBuffEffect
{
    #region 重要信息
    public int Id => 5; // 固定写死，对应表里ID
    public string Name => "魔流逆涌"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_AeverseManaSurge() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"你最后一颗子弹伤害-2，持续一场战斗";
    #endregion
}

public class Buff_IncantationKnot : IBuffEffect
{
    #region 重要信息
    public int Id => 6; // 固定写死，对应表里ID
    public string Name => "咒语打结"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_IncantationKnot() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"你最后一颗子弹伤害-2，持续一场战斗";
    #endregion
}

//100 + 
public class Buff_WeakenDefenses : IBuffEffect
{
    #region 重要信息
    public int Id => 100; // 固定写死，对应表里ID
    public string Name => "脆弱"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_WeakenDefenses() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"你的全部子弹伤害-1，持续一场战斗";
    #endregion
}

public class Buff_SpellLag : IBuffEffect
{
    #region 重要信息
    public int Id => 101; // 固定写死，对应表里ID
    public string Name => "咒式卡顿"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_SpellLag() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"第三颗子弹无法触发宝石效果，持续一场战斗";
    #endregion
}

//200 + 
public class Buff_DeflectedShot : IBuffEffect
{
    #region 重要信息
    public int Id => 201; // 固定写死，对应表里ID
    public string Name => "偏斜射击"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_DeflectedShot() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"穿透失效，持续一场战斗";
    #endregion
}

public class Buff_ResonanceBarrier : IBuffEffect
{
    #region 重要信息
    public int Id => 202; // 固定写死，对应表里ID
    public string Name => "共鸣屏障"; 
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;  // 触发时机
    public int Stack { get; set; } = 1;   // 当前层数
    public int RemainingBattles { get; set; } = 1; //持续几轮战斗则移除
    #endregion
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalDamage -= Stack;
    }
    #region 不关注的信息
    public bool IsExpired() => RemainingBattles == 0;
    public BuffData Data { get; private set; } // Buff基本信息
    public Buff_ResonanceBarrier() =>  Data = new BuffData(Id);
    public bool IsDebuff => Data.IsDebuff;
    public string GetDescription() => $"穿透失效，持续一场战斗";
    #endregion
}