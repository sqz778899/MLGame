using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TalentData
{
    // 配置数据
    public int ID;                   // 唯一ID
    public string Name;              // 天赋名称
    public int Level;                // 天赋等级
    public List<int> DependTalents;  // 依赖的天赋ID
    public List<int> UnlockTalents;  // 解锁的天赋ID
    public int Price;               // 价格
    
    // 动态数据
    public bool IsLocked;            // 是否锁定
    public bool IsLearned;          // 是否已学习
    
    public TalentData(int _id)
    {
        ID = _id;
        TalentJson curJson = TrunkManager.Instance.GetTalentJson(_id);
        Name = curJson.Name;
        Level = curJson.Level;
        DependTalents = curJson.DependTalents;
        UnlockTalents = curJson.UnlockTalents;
        Price = curJson.Price;

        if (_id != 1)
            IsLocked = true;
        else
            IsLocked = false;
        
        IsLearned = false;
    }
}
