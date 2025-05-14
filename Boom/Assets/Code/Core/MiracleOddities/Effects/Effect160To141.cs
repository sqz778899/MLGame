using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_DizzyOwlFigurine : IMiracleOddityEffect
{
    public int Id => 156; //错晕头转向的猫头鹰雕像 Dizzy Owl Figurine
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBattleStart;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.CurEnemy == null || ctx.CurEnemy.Shields == null) return;
        
        List<ShieldData> shieldData = ctx.CurEnemy.Shields;
        foreach (var shield in shieldData)
        {
            int randomChange = Random.Range(0, 2) == 0 ? -1 : 1; // ±1
            shield.ModifyHP(randomChange);
            // Debug.Log($"错晕头转向的猫头鹰雕像 {randomChange}");
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "战斗开始时，敌方护盾血量随机±1";
}