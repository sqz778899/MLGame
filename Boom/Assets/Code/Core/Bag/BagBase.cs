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
            Vector3 StandardOffsetItem = new Vector3(280f, -135f, 0);
            Vector3 StandardOffsetGemAndBullet = new Vector3(220f, -135f, 0);
            Vector3 NOffsetItem = new Vector3(-250f, -135f, 0);
            Vector3 NOffsetGemAndBullet = new Vector3(-200f, -135f, 0);
            return SlotType switch
            {
                SlotType.GemBagSlot => StandardOffsetGemAndBullet,
                SlotType.GemBagSlotInner => StandardOffsetGemAndBullet,
                SlotType.ItemBagSlot =>  StandardOffsetItem,
                SlotType.GemInlaySlot => NOffsetGemAndBullet,
                SlotType.SpawnnerSlot => StandardOffsetGemAndBullet,
                SlotType.SpawnnerSlotInner => StandardOffsetGemAndBullet,
                SlotType.CurBulletSlot => StandardOffsetGemAndBullet,
                SlotType.ShopSlot => StandardOffsetGemAndBullet,
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

    public void ClearTempBuffs() => ReturnGemType();

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
        if (index == -1) return;
        ID = targetList[index];
    }
    #endregion

    #region 不关心的初始化
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
        OnDataChanged?.Invoke();
    }
    Dictionary<GemType, List<int>> typeIDDict = new Dictionary<GemType, List<int>>
    {
        { GemType.Damage, new List<int>{1, 2, 3} },
        { GemType.Piercing,  new List<int>{10,11,12} },
        { GemType.Resonance, new List<int>{20,21,22} }
    };
    #endregion

    #region 额外功能
    //天赋针对的加成
    public void AddTalentGemBonus()
    {
        //同步一下天赋树加成
        List<TalentGemBonus> TalentGemBonuses = GM.Root.PlayerMgr._PlayerData.TalentGemBonuses;
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
    #endregion

    #region ToolTips相关
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
    
    public ToolTipsInfo BuildTooltipInBulletContext(BulletData bulletContext)
    {
        ToolTipsInfo info = new ToolTipsInfo(Name, Level, "", ToolTipsType.Gem);

        int gemAdditon = 0;
        int gemResonanceAdd = 0;

        foreach (var each in bulletContext.ModifierGemAdditionDict)
            gemAdditon += each.Value;
        foreach (var each in bulletContext.ModifierGemResonanceAdditionDict)
            gemResonanceAdd += each.Value;

        int dmg = Damage;
        int pierce = Piercing;
        int reso = Resonance;
        
        if (CurGemType == GemType.Damage)
            dmg += gemAdditon;
        if (CurGemType == GemType.Piercing)
            pierce += gemAdditon;
        if (CurGemType == GemType.Resonance)
            reso += gemAdditon + gemResonanceAdd;
        
        if((dmg + dmg - Damage) != 0)
            info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Damage, dmg, dmg - Damage));
        if((pierce + pierce - Piercing) != 0)
            info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Piercing, pierce, pierce - Piercing));
        if((reso + reso - Resonance) != 0)
            info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Resonance, reso, reso - Resonance));

        return info;
    }
    #endregion
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
    public int Critical;           //暴击率
    public int ElementalInfusionValue; //元素灌注值
    public ElementalTypes ElementalType;
    
    //动态数据层 运行时数据
    public int ResonanceDamage;
    public int FinalDamage;
    public int FinalPiercing;
    public int FinalResonance;
    public int FinalCritical;
    public int FinalElementalInfusionValue;
    public List<IBulletModifier> Modifiers = new();

    #region 针对buff道具等进行的属性增加
    public int OrderInRound; // 第几颗子弹（从1开始）
    public bool IsLastBullet; // 是否是最后一颗子弹
    public int JumpHitCount; // 跳跃命中次数
    public GemEffectOverrideState GemEffectOverride = GemEffectOverrideState.None; //宝石效果覆盖状态
    //针对宝石修改器的修改
    public Dictionary<string, int> ModifierGemAdditionDict = new(); //记录Buff对宝石附加值的修改
    public Dictionary<string, int> ModifierGemDamageAdditionDict = new(); //记录Buff对宝石附加值的修改(伤害专属)
    public Dictionary<string, int> ModifierGemPiercingAdditionDict = new(); //记录Buff对宝石附加值的修改(穿透专属)
    public Dictionary<string, int> ModifierGemResonanceAdditionDict = new(); //记录Buff对宝石附加值的修改(共振专属)
    //直接改Bullet本体
    public Dictionary<string, int> ModifierDamageAdditionDict = new(); //记录Buff直接对伤害的修改
    public Dictionary<string, int> ModifierPiercingAdditionDict = new(); //记录Buff直接对穿透的修改
    public Dictionary<string, int> ModifierResonanceAdditionDict = new(); //记录Buff直接对共振的修改
    #endregion

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
        SyncFinalAttributes();
    }
    #endregion

    #region 弹孵化器专用
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
    #endregion
    
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
        if (json == null) return;
        ID = json.ID;
        Level = json.Level;
        Name = json.Name;
        Price = json.Price;
        
        Damage = json.Damage;
        Piercing = json.Piercing;
        Resonance = json.Resonance;
        Critical = json.Critical;
        ElementalInfusionValue = json.ElementalInfusionValue;
        ElementalType = (ElementalTypes)json.ElementalType;
        SyncFinalAttributes();
    }
    
    public void SyncFinalAttributes()
    {
        FinalDamage = Damage;
        FinalPiercing = Piercing;
        FinalResonance = Resonance;
        FinalCritical = Critical;
        FinalElementalInfusionValue = ElementalInfusionValue;
        
        Modifiers.ForEach(mo => mo.Modify(this));//处理宝石
        //处理临时Buff
        foreach (var each in triggeredTempBuffs)
            each.Value.Apply(this);
        //处理全局Buff直接对子弹的修改
        foreach (var each in ModifierDamageAdditionDict)
            FinalDamage += each.Value;
        foreach (var each in ModifierPiercingAdditionDict)
            FinalPiercing += each.Value;
        foreach (var each in ModifierResonanceAdditionDict)
            FinalResonance += each.Value;
        
        if (FinalResonance == 0)
            ResonanceDamage = 0;
        FinalDamage += ResonanceDamage;

        FinalDamage = Mathf.Max(FinalDamage, 0);
        //BuildTooltip();
       // Debug.Log("BULLET FINAL DAMAGE: " + FinalDamage);
        OnDataChanged?.Invoke();
    }
    #endregion

    #region ToolTip等不关心的信息
    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name,Level,"", ToolTipsType.Bullet);

        if (Damage != 0 || FinalDamage != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Damage, FinalDamage,FinalDamage - Damage);
            info.AttriInfos.Add(curInfo);
        }
        if (Piercing != 0 || FinalPiercing != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Piercing, FinalPiercing,FinalPiercing - Piercing);
            info.AttriInfos.Add(curInfo);
        }
        if (Resonance != 0 || FinalResonance !=0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Resonance, FinalResonance,FinalResonance - Resonance);
            info.AttriInfos.Add(curInfo);
        }
        if (Critical != 0 || FinalCritical !=0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Critical, FinalCritical,FinalCritical - Critical);
            info.AttriInfos.Add(curInfo);
        }
        if (ElementalInfusionValue != 0 || FinalElementalInfusionValue !=0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.ElementalValue, FinalElementalInfusionValue,
                FinalElementalInfusionValue - ElementalInfusionValue);
            info.AttriInfos.Add(curInfo);
        }
        //把元素最后加上
        info.AttriInfos.Add(new ToolTipsAttriSingleInfo(ToolTipsAttriType.Element, elementType: ElementalType));
        return info;
    }
    
    public override ItemBaseSaveData ToSaveData() => new BulletBaseSaveData(this);
    #endregion
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
        int gemAdditon = 0;  //全效的宝石增益
        int gemDamageAdditon = 0; //专门针对伤害的增益
        int gemPiercingAdditon = 0; //专门针对穿透的增益
        int gemResonanceAdditon = 0; //专门针对共振的增益
        foreach (var each in data.ModifierGemAdditionDict)
            gemAdditon += each.Value;
        //专属伤害增益
        foreach (var each in data.ModifierGemDamageAdditionDict)
            gemDamageAdditon += each.Value;
        //专属穿透增益
        foreach (var each in data.ModifierGemPiercingAdditionDict)
            gemPiercingAdditon += each.Value;
        //专属共振增益
        foreach (var each in data.ModifierGemResonanceAdditionDict)
            gemResonanceAdditon += each.Value;

        int dmg = gem.Damage;
        int pierce = gem.Piercing;
        int reso = gem.Resonance;

        if (gem.CurGemType == GemType.Damage)
        {
            dmg += gemAdditon;
            dmg += gemDamageAdditon;
        }
        if (gem.CurGemType == GemType.Piercing)
        {
            pierce += gemAdditon;
            pierce += gemPiercingAdditon;
        }
        if (gem.CurGemType == GemType.Resonance)
        {
            reso += gemAdditon;
            reso += gemResonanceAdditon;
        }

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