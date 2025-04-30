using System;
using System.Collections.Generic;
using UnityEngine;

//基础抽象类
public interface IBulletModifier
{
    void Modify(BulletData data);
}
#region 接口
public interface IBindData
{
    void BindData(ItemDataBase data);
}

//高亮接口
public interface IHighlightableUI
{
    void SetHighlight(bool highlight);
}

//点击响应小接口
public interface IPressEffect
{
    void OnPressDown();
    void OnPressUp();
}
public interface ISlotController
{
    public SlotType SlotType { get; }
    public int SlotID { get; }
    public ItemDataBase CurData { get; }
    void Unassign();
    bool CanAccept(ItemDataBase data);
    void Assign(ItemDataBase data, GameObject itemGO);
    void AssignDirectly(ItemDataBase data, GameObject itemGO,bool isRefreshData = true);
    public Vector3 TooltipOffset{ get; }
    bool IsEmpty => CurData == null;
    public GameObject GetGameObject();
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
    public ISlotController CurSlotController;

    public virtual ToolTipsInfo BuildTooltip() { throw new NotImplementedException(); }
    public virtual ItemBaseSaveData ToSaveData() { throw new NotImplementedException(); }
}

//基础的SlotController类
public abstract class BaseSlotController<T> :ISlotController where T : ItemDataBase
{
    protected int _slotID;
    protected  SlotType _slotType;
    protected T _curData;
    protected GameObject CachedGO;
    public SlotView _view{get; private set;}
    
    // 实现 ISlotController 接口
    public SlotType SlotType => _slotType;
    public int SlotID => _slotID;
    public ItemDataBase CurData => _curData;

    public virtual void BindView(SlotView view) => _view = view;
    
    public void Init(int slotID, SlotType slotType)
    {
        _slotID = slotID;
        _slotType = slotType;
    }

    public virtual bool CanAccept(ItemDataBase data) { return data is T; }

    public virtual void Assign(ItemDataBase data, GameObject itemGO) {}
    public virtual void AssignDirectly(ItemDataBase data, GameObject itemGO,bool isRefreshData = true) {}
    public virtual void Unassign()
    {
        if (_curData != null)
            _curData.CurSlotController = null;

        _curData = null;
        CachedGO = null;
        _view?.Clear();
    }
    public Vector3 TooltipOffset
    {
        get
        {
            Vector3 StandardOffset = new Vector3(210f, -130f, 0);
            Vector3 NOffset = new Vector3(-190f, -130f, 0);
            return SlotType switch
            {
                SlotType.GemBagSlot => StandardOffset,
                SlotType.GemBagSlotInner => StandardOffset,
                SlotType.ItemBagSlot =>  StandardOffset,
                SlotType.GemInlaySlot => NOffset,
                SlotType.ItemEquipSlot => NOffset,
                SlotType.SpawnnerSlot => StandardOffset,
                SlotType.SpawnnerSlotInner => StandardOffset,
                SlotType.CurBulletSlot => StandardOffset,
                SlotType.ShopSlot => StandardOffset,
                _ => Vector3.zero
            };
        }
    }
    public GameObject GetGameObject() => CachedGO;
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
    public GemType CurGemType;
    public BulletModifierGem Modifier;
    
    #region 处理战场临时Buff
    public GemType ChangedType;
    Dictionary<string, bool> triggeredTempBuffs = new();
    public bool IsTriggered(string buffUniqueKey) => triggeredTempBuffs.ContainsKey(buffUniqueKey);
    //类型还原
    public void ReturnGemType()
    {
        if (triggeredTempBuffs.Count == 0)
            return;
        triggeredTempBuffs.Clear();
        List<int> s = typeIDDict[ChangedType];
        List<int> targetList = typeIDDict[CurGemType];
        int index = s.IndexOf(ID);
        ID = targetList[index];
    }

    public void ClearTempBuffs()
    {
        ReturnGemType();
    }

    //类型变化
    public void ChangeGemType(GemType _changedType,string buffUniqueKey)
    {
        // 已经触发过则跳过
        if (triggeredTempBuffs.ContainsKey(buffUniqueKey))
            return;
        
        triggeredTempBuffs[buffUniqueKey] = true;
        ChangedType = _changedType;
        List<int> s = typeIDDict[CurGemType];
        List<int> targetList = typeIDDict[_changedType];
        int index = s.IndexOf(ID);
        ID = targetList[index];
    }
    #endregion
    
    public GemData(int _id,GemSlotController gemSlotController)
    {
        GemJson json = TrunkManager.Instance.GetGemJson(_id);
        CurSlotController = gemSlotController;
        foreach (var each in typeIDDict)
        {
            if (each.Value.Contains(_id))
                CurGemType = each.Key;
        }
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
        SyncFinalAttributes();
    }
    Dictionary<GemType, List<int>> typeIDDict = new Dictionary<GemType, List<int>>
    {
        { GemType.Damage, new List<int>{1, 2, 3} },
        { GemType.Piercing,  new List<int>{10,11,12} },
        { GemType.Resonance, new List<int>{20,21,22} }
    };

    public void SyncFinalAttributes()
    {
        //处理临时Buff
       
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
    
    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name, Level,"", ToolTipsType.Gem);

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
public class BulletData:ItemDataBase,ITooltipBuilder
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
    //
    public int OrderInRound; // 第几颗子弹（从1开始）
    public int JumpHitCount; // 跳跃命中次数
    public bool IgnoreGemModifier = false; //是否忽略宝石加强(权重更高)
    public GemEffectOverrideState GemEffectOverride = GemEffectOverrideState.None; //宝石效果覆盖状态

    #region 处理战场临时Buff
    Dictionary<string, IBattleTempBuff> triggeredTempBuffs = new();
    public void AddTempBuff(IBattleTempBuff buff)
    {
        triggeredTempBuffs[buff.GetUniqueKey()] = buff;
        SyncFinalAttributes();
    }

    public void ClearTempBuffs()
    {
        triggeredTempBuffs.Clear();
        GemEffectOverride = GemEffectOverrideState.None;
        IgnoreGemModifier = false;
        SyncFinalAttributes();
    }
    #endregion
    
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
    
    public BulletData(int _id,BulletSlotController _slot)
    {
        BulletJson json = TrunkManager.Instance.GetBulletJson(_id);
        CurSlotController = _slot;
        JumpHitCount = 0;
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
        if (!IgnoreGemModifier)//处理临时Buff，让宝石修改失效
            Modifiers.ForEach(mo => mo.Modify(this));//处理宝石
        //处理临时Buff
        foreach (var each in triggeredTempBuffs)
            each.Value.Apply(this);
        
        if (FinalResonance == 0)
            ResonanceDamage = 0;
        FinalDamage += ResonanceDamage;
        OnDataChanged?.Invoke();
    }
    #endregion
    
    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name,Level,"", ToolTipsType.Bullet);

        if (FinalDamage != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Damage, FinalDamage,FinalDamage - Damage);
            info.AttriInfos.Add(curInfo);
        }
        if (FinalPiercing != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Piercing, FinalPiercing,FinalPiercing - Piercing);
            info.AttriInfos.Add(curInfo);
        }
        if (FinalResonance != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Resonance, FinalResonance,FinalResonance - Resonance);
            info.AttriInfos.Add(curInfo);
        }
        //把元素最后加上
        info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Element, elementType: ElementalType));
        return info;
    }
    
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
        int dmg = gem.Damage;
        int pierce = gem.Piercing;
        int reso = gem.Resonance;

        //宝石效果Buff相关
        switch (data.GemEffectOverride)
        {
            case GemEffectOverrideState.Ignore:
                return;
            case GemEffectOverrideState.Double:
                dmg *= 2;
                pierce *= 2;
                reso *= 2;
                break;
        }
        
        data.FinalDamage += dmg;
        data.FinalPiercing += pierce;
        data.FinalResonance += reso;
    }
}

public enum GemEffectOverrideState
{
    None = 0,
    Ignore = 1,
    Double = 2
}
#endregion