using UnityEngine;

public class Effect_CrazyHat: IItemEffect
{
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;

    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 0)
        {
            BulletData b = ctx.AllBullets[0];
            b.FinalDamage = Random.Range(1, 7);
        }
    }
    public string GetDescription() => "你第一个子弹的伤害随机变化（1～6），发射前无法预知。";
}

public class Effect_LuckyBoots : IItemEffect
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