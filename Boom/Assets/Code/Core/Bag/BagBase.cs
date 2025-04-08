using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public SlotController  CurSlotController;

    public virtual TooltipsInfo BuildTooltip() { throw new NotImplementedException(); }
    public virtual ItemBaseSaveData ToSaveData() { throw new NotImplementedException(); }
}
#endregion

#region 宝石类
public class GemData : ItemDataBase,ITooltipBuilder
{
    public event Action OnDataChanged;
    //静态数据层 配表数据
    public int Damage;
    public int Piercing;
    public int Resonance;
    public string ImageName;
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
        ImageName = json.ImageName;
        
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
    
    public TooltipsInfo BuildTooltip()
    {
        TooltipsInfo info = new TooltipsInfo(Name, Level);

        if (Damage != 0)
            info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Damage, Damage));
        if (Piercing != 0)
            info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Piercing, Piercing));
        if (Resonance != 0)
            info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Resonance, Resonance));

        return info;
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
        if (FinalResonance == 0)
            ResonanceDamage = 0;
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
    public string Desc;
    
    public IItemEffect EffectLogic; //运行时逻辑引用
    
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
        Desc = json.Desc;
        EffectLogic = ItemEffectFactory.CreateEffect(json.ID);
        OnDataChanged?.Invoke();
    }
    protected override void OnIDChanged()
    {
        ItemJson json = TrunkManager.Instance.GetItemJson(ID);
        InitData(json);
    }
}
#endregion