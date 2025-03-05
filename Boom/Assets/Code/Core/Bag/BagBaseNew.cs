using System;
using System.Collections.Generic;
using UnityEngine;

#region 接口
public interface ISaveable
{
    public ItemSaveData ToSaveData();
}

public interface IBulletModifier
{
    void Modify(BulletData data);
}
#endregion

[Serializable]
public abstract class ItemDataBase:ISaveable
{
    //静态数据层 配表数据
    [SerializeField] 
    int _id;
    public virtual int ID
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnIDChanged();
            }
        }
    }
    protected virtual void OnIDChanged() { }
    public string Name;
    public int Price;
    
    //动态数据层 运行时数据
    public int InstanceID;
    public int SlotID;
    public SlotType SlotType;
    
    public virtual ItemSaveData ToSaveData() { throw new NotImplementedException(); }
}

[Serializable]
public class ItemSaveData
{
    public int ID;          // 配表ID
    public int SlotID;      // 槽位ID
    public SlotType SlotType;

    public ItemSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
    public ItemSaveData(ItemBase item)
    {
        ID = item.ID;
        SlotID = item.SlotID;
        SlotType = item.SlotType;
    }
}

[Serializable]
public class BulletSaveData:ItemSaveData
{
    public int SpawnerCount;
    public BulletSaveData(BulletData data)
    {
        ID = data.ID; //有了ID，其他静态数据通过配表索引出来
        SlotID = data.SlotID;
        SlotType = data.SlotType;
        SpawnerCount = data.SpawnerCount;
    }
    public BulletSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}


#region 宝石类
public class GemData : ItemDataBase
{
    public int Damage;
    public int Piercing;
    public int Resonance;
    
    public GemData(GemJson json)
    {
        ID = json.ID;
        Name = json.Name;
        Price = json.Price;
        Damage = json.Attribute.Damage;
        Piercing = json.Attribute.Piercing;
        Resonance = json.Attribute.Resonance;
    }
}
#endregion


#region 子弹类
//动态数据层 运行时数据（数值计算、宝石镶嵌、元素关系）
public class BulletData:ItemDataBase
{
    public event Action OnDataChanged;
    //静态数据层 配表数据
    public int Level;
    public int Damage;
    public int Piercing;
    public int Resonance;
    public ElementalTypes ElementalType;
    
    //动态数据层 运行时数据
    public int FinalDamage;
    public int FinalPiercing;
    public int FinalResonance;
    public List<IBulletModifier> Modifiers = new();
    
    //子弹孵化器专用
    public int SpawnerCount;
    
    public BulletData(int _id)
    {
        BulletJson json = TrunkManager.Instance.GetBulletJson(_id);
        InitData(json);
    }

    #region 拓展插槽处理
    public void AddModifier(IBulletModifier modifier)
    {
        Modifiers.Add(modifier);
        SyncFinalAttributes();
        OnDataChanged?.Invoke();
    }
    
    public void ClearModifiers()
    {
        Modifiers.Clear();
        SyncFinalAttributes();
        OnDataChanged?.Invoke();
    }
    #endregion

    #region 数据同步
    protected override void OnIDChanged()
    {
        BulletJson json = TrunkManager.Instance.GetBulletJson(ID);
        InitData(json);
    }
    
    void InitData(BulletJson json)
    {
        ID = json.ID;
        Level = json.Level;
        Damage = json.Damage;
        Piercing = json.Piercing;
        Resonance = json.Resonance;
        ElementalType = (ElementalTypes)json.ElementalType;
        SyncFinalAttributes();
        OnDataChanged?.Invoke();
    }
    
    public void SyncFinalAttributes()
    {
        FinalDamage = Damage;
        FinalPiercing = Piercing;
        FinalResonance = Resonance;

        foreach (var modifier in Modifiers)
        {
            modifier.Modify(this);
        }
    }
    #endregion
    
    public override ItemSaveData ToSaveData() => new BulletSaveData(this);
}


//宝石修饰器
public class BulletModifierGem : IBulletModifier
{
    public GemData gem;

    public BulletModifierGem(GemData gem)
    {
        this.gem = gem;
    }

    public void Modify(BulletData data)
    {
        data.FinalDamage += gem.Damage;
        data.FinalPiercing += gem.Piercing;
        data.FinalResonance += gem.Resonance;
    }
}
#endregion