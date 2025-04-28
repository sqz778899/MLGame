using System;
using System.Collections.Generic;
using UnityEngine;


//特质接口
public interface IItemSynergies
{
    string Name { get; }
    string Description { get; }
    bool Match(List<ItemData> equippedItems); // 判断是否满足条件
    void ApplyEffect(BattleContext ctx); // 应用效果（支持不同阶段）
    ItemTriggerTiming TriggerTiming { get; }
    
    Sprite GetIcon(); // 获取图标
}

#region 道具类
public class ItemData : ItemDataBase,ITooltipBuilder
{
    public event Action OnDataChanged;
    public int ID;
    public string Name;
    public string Desc;
    public string ImageName;
    public DropedRarity Rarity;

    public IItemEffect EffectLogic; // 每个道具一个策略实现
    //区分是否是任务物品
    public ItemCategory Category { get; private set; }

    #region 物品堆叠支持
    public PersistentItemType PersistentType { get; private set; }
    //堆叠数量
    int _stackCount;
    public int StackCount
    {
        get => _stackCount;
        set
        {
            if (_stackCount != value)
            {
                _stackCount = value;
                OnDataChanged?.Invoke();
            }
        }
    }
    public int MaxStackCount; // 最大堆叠数量
    public bool IsStackable => Category == ItemCategory.Persistent && PersistentType == PersistentItemType.Resource;
    #endregion
    public bool IsEquipable => Category == ItemCategory.Equipable;
    public bool IsPersistent => Category == ItemCategory.Persistent;

    public ItemData(int id,ItemSlotController itemSlotController)
    {
        var json = TrunkManager.Instance.GetItemJson(id);
        CurSlotController = itemSlotController;
        ID = json.ID;
        Name = json.Name;
        Desc = json.Desc;
        Rarity = json.Rarity;
        Category = json.Category;
        PersistentType = json.PersistentType;
        ImageName = json.ResName;
        EffectLogic = ItemEffectFactory.CreateEffectLogic(ID);
        
        // 新加的
        //MaxStackCount = json.MaxStackCount > 0 ? json.MaxStackCount : 1; // 读配置
        MaxStackCount = 5;
        StackCount = 1;
    }

    public void ApplyEffect(BattleContext ctx) => EffectLogic?.Apply(ctx);

    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name,Level,Desc, ToolTipsType.Item,
            Rarity,Category,PersistentType);
        return info;
    }
}

public class BattleContext
{
    public List<BulletData> AllBullets;
    public EnemyData CurEnemy;
    public int RoundIndex;
    public bool IsTreasureRoom;
}

public enum ItemTriggerTiming
{
    OnAlltimes = 0,
    OnBattleStart = 1,
    OnBulletFire = 2,
    OnBulletHit = 3,
    OnShieldPenetrate = 4,
    OnEnterTreasureRoom = 5,
    Passive = 6,
}

//用于UI显示的特质的信息结构体
public class ItemComboSynergiesInfo
{
    public string Name;
    public string TraitDesc;
    public Sprite Icon;

    public ItemComboSynergiesInfo(string name, string desc, Sprite icon)
    {
        Name = name;
        TraitDesc = desc;
        Icon = icon;
    }
}
#endregion