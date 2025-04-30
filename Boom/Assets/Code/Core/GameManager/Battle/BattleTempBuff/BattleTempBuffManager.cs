using System;
using System.Collections.Generic;
using Sirenix.Utilities;

public interface IBattleTempBuff
{
    BuffSource Source { get; }
    int SourceID { get; }
    void Apply(BulletData bullets); // 通用接口，目标对象为子弹列表
    void ApplyMacro();   // 宏观层需要调用
    void RemoveBuff();
    string GetUniqueKey();
}

public class BattleTempBuffManager
{
    Dictionary<string,IBattleTempBuff> activeBuffs = new();
    List<GemData> gems => GM.Root.InventoryMgr._InventoryData.EquipGems;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    
    public void Add(IBattleTempBuff buff)
    {
        //先去重
        activeBuffs[buff.GetUniqueKey()] = buff;
        ApplyAll();
    }

    /// 应用到当前所有子弹（每次修改顺序/拖拽时都应调用）
    public void ApplyAll()
    {
        foreach (var each in activeBuffs)
        {
            foreach (var eachBullet in bullets)
                eachBullet.AddTempBuff(each.Value);
            each.Value.ApplyMacro();
        }
    }
    
    public void Clear()
    {
        bullets.ForEach(b => b.ClearTempBuffs());
        gems.ForEach(g=> g.ClearTempBuffs());
        activeBuffs.ForEach(b => b.Value.RemoveBuff());
        activeBuffs.Clear();
    }
}