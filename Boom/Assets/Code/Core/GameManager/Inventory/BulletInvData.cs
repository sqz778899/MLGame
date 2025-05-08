using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class BulletInvData : ScriptableObject
{
    public List<BulletData> BagBulletSpawners = new();//全部的子弹
    public List<BulletData> EquipBullets = new();// 有顺序要求的装备子弹
    public List<BulletSlotController> CurBulletSlotControllers;//子弹槽
    
    public event Action OnBulletsChanged;//子弹数据变化
    public event Action OnModifiersChanged;//子弹数据变化
    
    public void ClearData()
    {
        foreach (var each in BagBulletSpawners)
            each.SpawnerCount = 0;
        EquipBullets.Clear();
        OnBulletsChanged?.Invoke();
    }

    //宝石操作和子弹槽操作都会响应这个函数
    public void RefreshModifiers()
    {
        foreach (var bulletData in EquipBullets)
        {
            bulletData.ClearModifiers();
            BulletSlotController mainController = CurBulletSlotControllers.FirstOrDefault(
                b => b.SlotID == bulletData.CurSlotController.SlotID);
            foreach (var eGemSlotController in mainController.GemSlotControllers)
            {
                if(eGemSlotController.CurData == null)
                    continue;
                GemData gemData = eGemSlotController.CurData as GemData;
                bulletData.AddModifier(new BulletModifierGem(gemData));
            }
        }
        
        ProcessBulletRelations();
        OnModifiersChanged?.Invoke();
    }
    
    #region 子弹操作
    public void EquipBullet(BulletData bulletData)
    {
        if (EquipBullets.Count >= 5) return;
        if (!EquipBullets.Contains(bulletData))
            EquipBullets.Add(bulletData);
        RefreshModifiers();
        SortEquipBullet();//子弹内部数据进行排序
    }
    
    public void UnEquipBullet(BulletData _data)
    {
        if (_data == null) return;
        EquipBullets.Remove(_data);
        RefreshModifiers();
        SortEquipBullet();//子弹内部数据进行排序
        OnBulletsChanged?.Invoke();
    }
    
    public void AddSpawner(int bulletID)
    {
        BulletData spawner = BagBulletSpawners.FirstOrDefault(each => each.ID == bulletID);
        if (spawner != null) spawner.SpawnerCount++;
    }
    
    public void SortEquipBullet()
    {
        EquipBullets.Sort((bullet1, bullet2) => 
            bullet1.CurSlotController.SlotID.CompareTo(bullet2.CurSlotController.SlotID));
        //排序完后，依次写入 OrderInRound
        for (int i = 0; i < EquipBullets.Count; i++)
        {
            EquipBullets[i].OrderInRound = i + 1;
            EquipBullets[i].IsLastBullet = false;
            if (i == EquipBullets.Count -1)
                EquipBullets[i].IsLastBullet = true;
        }
        OnBulletsChanged?.Invoke();
    }
    #endregion

    #region 子弹共振关系处理（单独方法封装）
    public void ProcessBulletRelations()
    {
        if (EquipBullets.Count < 2) return;

        SortEquipBullet();
        ResonanceSlotCol[] ResonanceSlotCols= EternalCavans.Instance.
            EquipBulletSlotRoot.GetComponentsInChildren<ResonanceSlotCol>(true);
      
        //处理共振
        for (int i = 1; i < EquipBullets.Count; i++)
            EquipBullets[i].ResonanceDamage = 0;
        
        Dictionary<int,List<int>> ResonanceClusterDict = new Dictionary<int, List<int>>();
        int clusterCount = 1;
        int resonanceCount = 0;
        for (int i = 1; i < EquipBullets.Count; i++)
        {
            BulletData preBullet = EquipBullets[i - 1];
            BulletData nextBullet = EquipBullets[i];
            if (preBullet.FinalResonance == 0 || nextBullet.FinalResonance == 0)//不符合共振条件
            {
                resonanceCount = 0;
                continue;//不符合共振条件
            }

            /*bool isResonance = false;
            int preRemainder = preBullet.ID % 100;
            int nextRemainder = nextBullet.ID % 100;
            if (nextRemainder == preRemainder)//符合共振条件
            {
                resonanceCount++;
                preBullet.IsResonance = true;
                nextBullet.IsResonance = true;
                nextBullet.ResonanceDamage = 0;
                nextBullet.ResonanceDamage += nextBullet.FinalResonance * resonanceCount;
                nextBullet.SyncFinalAttributes();
                //构建共振簇
                if (ResonanceClusterDict.ContainsKey(clusterCount))
                    ResonanceClusterDict[clusterCount].Add(nextBullet.CurSlotController.SlotID);
                else
                    ResonanceClusterDict[clusterCount] = new List<int>{preBullet.CurSlotController.SlotID,nextBullet.CurSlotController.SlotID};
            }
            else
                resonanceCount = 0;*/
            
            resonanceCount++;
            nextBullet.ResonanceDamage = 0;
            nextBullet.ResonanceDamage += nextBullet.FinalResonance * resonanceCount;
            nextBullet.SyncFinalAttributes();
            //构建共振簇
            if (ResonanceClusterDict.ContainsKey(clusterCount))
                ResonanceClusterDict[clusterCount].Add(nextBullet.CurSlotController.SlotID);
            else
                ResonanceClusterDict[clusterCount] = new List<int>{preBullet.CurSlotController.SlotID,nextBullet.CurSlotController.SlotID};

            if (resonanceCount == 0)//说明共振被中断了，要重新开始
                clusterCount++;
        }
        
        //处理共振簇
        ResonanceSlotCols.ForEach(s => s.CloseEffect());
        //return;
        foreach (var each in ResonanceClusterDict)
        {
            foreach (var slotCol in ResonanceSlotCols)
            {
                if (each.Value.Count != slotCol.ResonanceSlots.Count) continue;

                if (each.Value.OrderBy(x => x).SequenceEqual(slotCol.ResonanceSlots.OrderBy(x => x)))
                {
                    slotCol.OpenEffect();
                }
            }
        }
    }
    #endregion
}
