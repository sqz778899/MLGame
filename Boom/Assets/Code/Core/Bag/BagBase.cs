using System;
using System.Collections.Generic;
using UnityEngine;

#region 接口
public interface IBulletModifier
{
    void Modify(BulletData data);
}

public interface IBindData
{
    void BindData(ItemDataBase data);
}
#endregion

#region 基础抽象类
//基础抽象类
public abstract class ItemBase : MonoBehaviour,IBindData
{
    public virtual void BindData(ItemDataBase data){}
}

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

    public int Level;
    protected virtual void OnIDChanged() { }
    public string Name;
    public int Price;
    
    //动态数据层 运行时数据
    public int InstanceID;
    public SlotBase CurSlot;
    
    public virtual ItemBaseSaveData ToSaveData() { throw new NotImplementedException(); }
}
#endregion

#region 宝石类
public class GemData : ItemDataBase
{
    public event Action OnDataChanged;
    //静态数据层 配表数据
    public int Damage;
    public int Piercing;
    public int Resonance;
    public BulletModifierGem Modifier;
    
    public GemData(int _id,SlotBase _slot)
    {
        GemJson json = TrunkManager.Instance.GetGemJson(_id);
        CurSlot = _slot;
        InitData(json);
        Modifier = new BulletModifierGem(this);
    }

    public void InitData(GemJson json)
    {
        ID = json.ID;
        Name = json.Name;
        Price = json.Price;
        Level = json.Level;
        
        Damage = json.Damage;
        Piercing = json.Piercing;
        Resonance = json.Resonance;
        OnDataChanged?.Invoke();
    }
    
    public void AddTalentGemBonus()
    {
        //同步一下天赋树加成
        List<TalentGemBonus> TalentGemBonuses = PlayerManager.Instance._PlayerData.TalentGemBonuses;
        foreach (var bonus in TalentGemBonuses)
        {
            if (bonus.GemID == ID)
            {
                if (ID < 10)
                    Damage += bonus.BonusValue;
                else if (ID < 20)
                    Piercing += bonus.BonusValue;
                else if (ID < 30)
                    Resonance += bonus.BonusValue;
            }
        }
        OnDataChanged?.Invoke();
    }
    
    protected override void OnIDChanged()
    {
        GemJson json = TrunkManager.Instance.GetGemJson(ID);
        InitData(json);
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
    public int ResonanceDamage;
    public int FinalDamage;
    public int FinalPiercing;
    public int FinalResonance;
    public bool IsResonance; //是否开启共振
    public List<IBulletModifier> Modifiers = new();
    
    //子弹孵化器专用
    public int _spawnerCount;
    public int SpawnerCount
    {
        get => _spawnerCount;
        set
        {
            _spawnerCount = value;
            OnDataChanged?.Invoke();
        }
    }
    
    public BulletData(int _id,SlotBase _slot)
    {
        BulletJson json = TrunkManager.Instance.GetBulletJson(_id);
        CurSlot = _slot;
        InitData(json);
    }

    #region 拓展插槽处理
    public void AddModifier(IBulletModifier modifier)
    {
        Modifiers.Add(modifier);
        SyncFinalAttributes();
    }
    
    public void ClearModifiers()
    {
        Modifiers.Clear();
        SyncFinalAttributes();
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
        Name = json.Name;
        Price = json.Price;
        
        Damage = json.Damage;
        Piercing = json.Piercing;
        Resonance = json.Resonance;
        
        ElementalType = (ElementalTypes)json.ElementalType;
        SyncFinalAttributes();
    }
    
    public void SyncFinalAttributes()
    {
        FinalDamage = Damage;
        FinalPiercing = Piercing;
        FinalResonance = Resonance;
        Modifiers.ForEach(mo => mo.Modify(this));
        
        FinalDamage += ResonanceDamage;
        OnDataChanged?.Invoke();
    }
    #endregion
    
    public override ItemBaseSaveData ToSaveData() => new BulletBaseSaveData(this);
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

#region 道具类
public class ItemData : ItemDataBase
{
    public event Action OnDataChanged;
    //静态数据层 配表数据
    public int Rare;
    public int Damage;
    public int Piercing;
    public int Resonance;
    
    public ItemData(int _id,SlotBase _slot)
    {
        ItemJson json = TrunkManager.Instance.GetItemJson(_id);
        CurSlot = _slot;
        InitData(json);
    }

    public void InitData(ItemJson json)
    {
        ID = json.ID;
        Name = json.Name;
        Price = json.Price;
        Rare = json.Rare;
        OnDataChanged?.Invoke();
    }
    protected override void OnIDChanged()
    {
        ItemJson json = TrunkManager.Instance.GetItemJson(ID);
        InitData(json);
    }
}

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
#endregion