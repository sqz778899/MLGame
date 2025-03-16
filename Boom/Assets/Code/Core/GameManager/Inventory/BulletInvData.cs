using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="Inventory/BulletInventory")]
public class BulletInvData : ScriptableObject
{
    public List<BulletData> BagBulletSpawners = new();//全部的子弹
    public List<BulletData> EquipBullets = new();// 有顺序要求的装备子弹
    public List<BulletSlotRole> EquipSlots = new(); //子弹槽
    
    // 子弹变动事件
    public event Action OnBulletsChanged;
    
    #region 子弹操作
    public void AddSpawner(int bulletID)
    {
        BulletData spawner = BagBulletSpawners.FirstOrDefault(each => each.ID == bulletID);
        if (spawner != null) spawner.SpawnerCount++;
    }
    
    public void UnEquipBullet(BulletData _data)
    {
        if (_data == null) return;
        RemoveEquipBullet(_data);
        AddSpawner(_data.ID);
        OnBulletsChanged?.Invoke();
    }

    public void EquipBullet(BulletData bulletData)
    {
        if (EquipBullets.Count >= 5) return;
        if (!EquipBullets.Contains(bulletData))
            EquipBullets.Add(bulletData);
        SortEquipBullet();
    }
    
    public void SortEquipBullet()
    {
        EquipBullets.Sort((bullet1, bullet2) => 
            bullet1.CurSlot.SlotID.CompareTo(bullet2.CurSlot.SlotID));
        OnBulletsChanged?.Invoke();
    }

    public void RemoveEquipBullet(BulletData _data)
    {
        EquipBullets.Remove(_data);
        OnBulletsChanged?.Invoke();
    }
    #endregion

    #region 子弹共振关系处理（单独方法封装）
    public void ProcessBulletRelations()
    {
        if (EquipBullets.Count < 2) return;

        ResonanceSlotCol[] ResonanceSlotCols = UIManager.Instance.BagUI.EquipBulletSlotRoot.
            GetComponentsInChildren<ResonanceSlotCol>();
        //处理共振
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

            bool isResonance = false;
            int preRemainder = preBullet.ID % 100;
            int nextRemainder = nextBullet.ID % 100;
            if (nextRemainder == preRemainder)//符合共振条件
            {
                resonanceCount++;
                //preBullet.IsResonance = true;
                //nextBullet.IsResonance = true;
                //CurBulletsPair[preBullet].IsResonance = true;
                //开始添加共振伤害
                /*Bullet nextBulletSC = CurBulletsPair[nextBullet];
                nextBulletSC.IsResonance = true;
                nextBulletSC.FinalDamage += nextBulletSC.FinalResonance * resonanceCount;
                nextBullet.FinalDamage += nextBullet.FinalResonance * resonanceCount;*/
                //构建共振簇
                if (ResonanceClusterDict.ContainsKey(clusterCount))
                    ResonanceClusterDict[clusterCount].Add(nextBullet.CurSlot.SlotID);
                else
                    ResonanceClusterDict[clusterCount] = new List<int>{preBullet.CurSlot.SlotID,nextBullet.CurSlot.SlotID};
            }
            else
            {
                resonanceCount = 0;
            }

            if (resonanceCount == 0)//说明共振被中断了，要重新开始
            {
                clusterCount++;
            }
        }
        
        //处理共振簇
        foreach (var slotCol in ResonanceSlotCols)
        {
            slotCol.CloseEffect();
        }

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
