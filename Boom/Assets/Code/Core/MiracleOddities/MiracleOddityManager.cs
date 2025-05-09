using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiracleOddityManager
{
    //道具
    List<MiracleOddityData> equipMiracleOddities => GM.Root.InventoryMgr._InventoryData.EquipMiracleOddities;
    List<MiracleOddityData> _activeAllTimeMiracleOddities = new();

    public void InitData()
    {
        BattleEventBus.OnFire -= TriggerOnBulletFire;
        BattleEventBus.OnFire += TriggerOnBulletFire;
        ApplyAlltimesEffectsToBullets();
    }
    //触发开火时奇迹物件特性
    public void TriggerOnBulletFire() => Trigger(MiracleOddityTriggerTiming.OnBulletFire);

    //常规根据Timing触发
    public void Trigger(MiracleOddityTriggerTiming timing,BattleContext ctx = null)
    {
        ctx ??= new BattleContext();
        #region 时态道具触发
        //触发每一个道具的效果
        foreach (var item in equipMiracleOddities)
        {
            if (item.EffectLogic?.TriggerTiming == timing)
                item.ApplyEffect(ctx);
        }
        #endregion
    }
    
    //根据Timming触发道具的Cash信息
    public void TriggerCash(MiracleOddityTriggerTiming timing,BattleContext ctx = null)
    {
        ctx ??= new BattleContext();
        //触发每一个道具的Cash信息
        foreach (var item in equipMiracleOddities)
        {
            if (item.EffectLogic?.TriggerCash == timing)
                item.ApplyEffectCash(ctx);
        }
    }
    
    public void ApplyAlltimesEffectsToBullets()
    {
        foreach (var each in _activeAllTimeMiracleOddities)
            each.RemoveEffect();
        
        foreach (var moData in equipMiracleOddities)
        {
            if (moData.EffectLogic?.TriggerTiming == MiracleOddityTriggerTiming.OnAlltimes)
            {
                _activeAllTimeMiracleOddities.Add(moData);
                moData.ApplyEffect(new BattleContext());
            }
        }
    }
    
    public bool ShouldRetryGambling()
    {
        foreach (var item in equipMiracleOddities)
        {
            if (item.EffectLogic?.TriggerTiming == MiracleOddityTriggerTiming.OnAlltimes &&
                item.ID == 204) // Scavenger's Handbook
            {
                return UnityEngine.Random.value < 0.5f;
            }
        }
        return false;
    }
}