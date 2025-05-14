using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Effect_EchoingEdict : IMiracleOddityEffect
{
    #region 数据
    public int Id => 99; //永响之谕 Echoing Edict
    string cacheKey => $"永响之谕-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if(i == bullets.Count - 1)
                bullets[i].ModifierDamageAdditionDict[cacheKey] = 5;
            else
                bullets[i].ModifierDamageAdditionDict[cacheKey] = 0;
            bullets[i].SyncFinalAttributes();
        }
        //Debug.Log($"[永响之谕] 触发");
    }
    public void RemoveEffect()
    {
        foreach (var each in bullets)
        {
            each.ModifierDamageAdditionDict.Remove(cacheKey);
            each.SyncFinalAttributes();
        }
    }
    #endregion
    public string GetDescription() => "最后一颗子弹伤害#Red(+5)#";
}
