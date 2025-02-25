using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

//基础抽象类
public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] int _id;
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
    public SlotBase CurSlot; //当前拖拽物所在的Slot
    public string Name;
    public int InstanceID;
    public int SlotID;
    public SlotType SlotType;

    public abstract void SyncData();
}

// 统一 Json 数据基类
public abstract class ItemJsonBase
{
    public int ID;
    public int InstanceID;
    public string Name;
    public int SlotID;
    public int SlotType;
    public int Price;
}


#region 宝石类
//和GO解绑的纯数据
[Serializable]
public class GemAttribute
{
    public int Damage;
    public int Piercing;
    public int Resonance;
    
    public GemAttribute(int damage = 0, int piercing = 0, int resonance = 0)
    {
        Damage = damage;
        Piercing = piercing;
        Resonance = resonance;
    }
}

[Serializable]
public class GemJson : ItemJsonBase
{
    public int Level;
    public string ImageName;
    public GemAttribute Attribute;
    public bool IsInLay;
    public int BulletSlotIndex;
    
    public GemJson(int id = -1, int instanceID = -1, 
        string name = "", GemAttribute attribute = null, 
        int _level = 1,string imageName = "", int slotID = -1, int slotType = -1)
    {
        ID = id;
        InstanceID = instanceID;
        Name = name;
        if (attribute == null)
            attribute = new GemAttribute();
        Attribute = attribute;
        Level = _level;
        ImageName = imageName;
        SlotID = slotID;
        SlotType = slotType;
        //是否镶嵌
        IsInLay = false;
        BulletSlotIndex = -1;
        Price = 0;
    }
    
    public void CopyFrom(GemJson other)
    {
        if (other == null) return;

        ID = other.ID;
        InstanceID = other.InstanceID;
        Name = other.Name;
        Attribute = new GemAttribute(other.Attribute.Damage,
            other.Attribute.Piercing,other.Attribute.Resonance);  // 假设 GemAttribute 有复制构造函数
        Level = other.Level;
        ImageName = other.ImageName;
        SlotID = other.SlotID;
        SlotType = other.SlotType;
        IsInLay = other.IsInLay;
        BulletSlotIndex = other.BulletSlotIndex;
        Price = other.Price;
    }
}
#endregion

#region 道具类
[Serializable]
public class ItemAttribute
{
    public int waterElement;
    public int fireElement;
    public int thunderElement;
    public int lightElement;
    public int darkElement;

    public int extraWaterDamage;
    public int extraFireDamage;
    public int extraThunderDamage;
    public int extraLightDamage;
    public int extraDarkDamage;
    
    public int maxDamage;

    public ItemAttribute(int _waterElement = 0, int _fireElement= 0, int _thunderElement= 0,
        int _lightElement = 0, int _darkElement = 0,int _extraWaterDamage = 0,
        int _extraFireDamage = 0,int _extraThunderDamage = 0,int _extraLightDamage = 0,
        int _extraDarkDamage = 0,int _maxDamage = 0)
    {
        waterElement = _waterElement;
        fireElement = _fireElement;
        thunderElement = _thunderElement;
        lightElement = _lightElement;
        darkElement = _darkElement;
        
        //
        extraWaterDamage = _extraWaterDamage;
        extraFireDamage = _extraFireDamage;
        extraThunderDamage = _extraThunderDamage;
        extraLightDamage = _extraLightDamage;
        extraDarkDamage = _extraDarkDamage;
        maxDamage = _maxDamage;
    }
    
    // 聚合其他 ItemAttribute 对象的属性
    public void Aggregate(ItemJson other)
    {
        waterElement += other.Attribute.waterElement;
        fireElement += other.Attribute.fireElement;
        thunderElement += other.Attribute.thunderElement;
        lightElement += other.Attribute.lightElement;
        darkElement += other.Attribute.darkElement;

        extraWaterDamage += other.Attribute.extraWaterDamage;
        extraFireDamage += other.Attribute.extraFireDamage;
        extraThunderDamage += other.Attribute.extraThunderDamage;
        extraLightDamage += other.Attribute.extraLightDamage;
        extraDarkDamage += other.Attribute.extraDarkDamage;

        maxDamage += other.Attribute.maxDamage;
    }
}

[Serializable]
public class ItemJson:ItemJsonBase
{
    public int Rare;
    public string ImageName;
    public ItemAttribute Attribute;
    
    public ItemJson(int _id = -1, int _instanceID = -1,
        int _rare = -1, string _name = "", 
        string _imageName = "", ItemAttribute _attribute = null,
        int _SlotID = 0, int _SlotType = 0)
    {
        ID = _id;
        InstanceID = _instanceID;
        Rare = _rare;
        Name = _name;
        ImageName = _imageName;
        if (_attribute == null)
        {
            _attribute = new ItemAttribute();
        }
        Attribute = _attribute;
        SlotID = _SlotID;
        SlotType = _SlotType;
        Price = 0;
    }
    
    public void CopyFrom(ItemJson other)
    {
        if (other == null) return;

        // 复制基础字段
        this.ID = other.ID;
        this.InstanceID = other.InstanceID;
        this.Rare = other.Rare;
        this.Name = other.Name;
        this.ImageName = other.ImageName;
        this.SlotID = other.SlotID;
        this.SlotType = other.SlotType;

        // 复制 Attribute
        // 如果 Attribute 不是 null，那么我们直接聚合它，假如你是想合并多个 ItemJson 的属性，调用 Aggregate。
        // 否则，直接赋值。
        if (other.Attribute != null)
        {
            this.Attribute = new ItemAttribute(
                other.Attribute.waterElement,
                other.Attribute.fireElement,
                other.Attribute.thunderElement,
                other.Attribute.lightElement,
                other.Attribute.darkElement,
                other.Attribute.extraWaterDamage,
                other.Attribute.extraFireDamage,
                other.Attribute.extraThunderDamage,
                other.Attribute.extraLightDamage,
                other.Attribute.extraDarkDamage,
                other.Attribute.maxDamage
            );
        }
    }
}
#endregion

#region 子弹类
[Serializable]
public class BulletJson : ItemJsonBase
{
    public int Level;
    public int ElementalType;
    
    //基础属性
    public int Damage;
    public int Piercing;
    public int Resonance;
    //额外属性
    public int FinalDamage;
    public int FinalPiercing;
    public int FinalResonance;

    public bool IsResonance = false;
    //如果是Spawner，则有Count
    public int SpawnerCount;
    
    public string HitEffectName;

    public   BulletJson(int _id = -1, int _instanceID = -1, string _name = "", 
        int _slotID = -1, int _slotType = -1,int _level = 1,int _elementalType = -1,
        int _damage = 0, int _piercing = 0, int _resonance = 0,int _spawnerCount = 0
        ,string _hitEffectName = "")
    {
        ID = _id;
        InstanceID = _instanceID;
        Name = _name;
        SlotID = _slotID;
        SlotType = _slotType;
        Level = _level;
        ElementalType = _elementalType;
        Damage = _damage;
        Piercing = _piercing;
        Resonance = _resonance;
        SpawnerCount = _spawnerCount;
        HitEffectName = _hitEffectName;
        Price = 0;
    }

    public void CopyFrom(BulletJson other)
    {
        if (other == null) return;

        // 复制基础字段
        this.ID = other.ID;
        this.InstanceID = other.InstanceID;
        this.Name = other.Name;
        this.SlotID = other.SlotID;
        this.SlotType = other.SlotType;
        this.Level = other.Level;
        this.ElementalType = other.ElementalType;
        this.Damage = other.Damage;
        this.Piercing = other.Piercing;
        this.Resonance = other.Resonance;
        this.FinalDamage = other.FinalDamage;
        this.FinalPiercing = other.FinalPiercing;
        this.FinalResonance = other.FinalResonance;
        this.IsResonance = other.IsResonance;
        this.SpawnerCount = other.SpawnerCount;
        this.HitEffectName = other.HitEffectName;
    }

    public void SyncData()
    {
        BulletJson designJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(each => each.ID == ID) ?? new BulletJson();
        
        // 通过反射遍历所有字段并同步值
        foreach (var field in typeof(BulletJson).GetFields())
        {
            field.SetValue(this, field.GetValue(designJson));
        }
    }
}
#endregion
