using System;
using System.Collections.Generic;
using UnityEngine;

//基础抽象类
#region 接口
public interface IBulletModifier
{
    void Modify(BulletData data);
}
public interface IBindData
{
    void BindData(ItemDataBase data);
}

public interface ISlotController
{
    public SlotType SlotType { get; }
    public int SlotID { get; }
    public ItemDataBase CurData { get; }
    void Unassign();
    bool CanAccept(ItemDataBase data);
    void Assign(ItemDataBase data, GameObject itemGO);
    void AssignDirectly(ItemDataBase data, GameObject itemGO);
    public Vector3 TooltipOffset{ get; }
    bool IsEmpty => CurData == null;
    bool IsCameraNear { get; set; }
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
    public virtual void AssignDirectly(ItemDataBase data, GameObject itemGO) {}
    public virtual void Unassign()
    {
        if (_curData != null)
            _curData.CurSlotController = null;

        _curData = null;
        CachedGO = null;
        _view?.Clear();
    }
    public bool IsCameraNear { get; set; }//是否摄像机切近景了
    public Vector3 TooltipOffset
    {
        get
        {
            return SlotType switch
            {
                SlotType.GemBagSlot =>IsCameraNear?
                    new Vector3(0.7f, -0.39f, 0) :
                    new Vector3(1.01f, -0.6f, 0),
                SlotType.GemBagSlotInner=>IsCameraNear?
                    new Vector3(0.7f, -0.39f, 0) :
                    new Vector3(1.01f, -0.6f, 0),
                SlotType.ItemBagSlot => new Vector3(1.2f, -0.6f, 0),
                SlotType.GemInlaySlot => new Vector3(-0.92f, -0.6f, 0),
                SlotType.ItemEquipSlot => new Vector3(-1.1f, -0.6f, 0),
                SlotType.SpawnnerSlot => new Vector3(1.01f, -0.6f, 0),
                SlotType.SpawnnerSlotInner =>IsCameraNear?
                    new Vector3(0.7f, -0.39f, 0) :
                    new Vector3(1.01f, -0.6f, 0),
                SlotType.CurBulletSlot => new Vector3(1.01f, -0.6f, 0),
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
    public BulletModifierGem Modifier;
    
    public GemData(int _id,GemSlotController gemSlotController)
    {
        GemJson json = TrunkManager.Instance.GetGemJson(_id);
        CurSlotController = gemSlotController;
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
        data.FinalDamage += gem.Damage;
        data.FinalPiercing += gem.Piercing;
        data.FinalResonance += gem.Resonance;
    }
}
#endregion