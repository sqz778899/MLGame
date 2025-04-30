using System;
using System.Collections.Generic;
using UnityEngine;


//特质接口
public interface IItemSynergies
{
    int Id { get; } // 新增：特质 ID
    TraitData Data { get; } // 新增：特质 Data（懒加载）
    
    string Name { get; }
    string Description { get; }
    bool Match(List<ItemData> equippedItems); // 判断是否满足条件
    void ApplyEffect(BattleContext ctx); // 应用效果（支持不同阶段）
    ItemTriggerTiming TriggerTiming { get; }
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
    public EnemyData CurEnemy; // 当前敌人
    public BulletData CurBullet; // 当前命中的子弹
    public IDamageable TargetEnemy; // 当前命中的敌人
    public int OrderInRound => CurBullet?.OrderInRound ?? -1;//当前子弹的顺序
    //进入房间相关
    public int EnterRoomID;
    public bool IsFirstEnterRoom;
    // 是否跳过命中
    public bool ShieldSkipCount;

    public BattleContext() => InitCommonData();
    
    public BattleContext(BulletData _curBullet,IDamageable _curTargetEnemy)
    {
        InitCommonData();
        CurBullet = _curBullet;
        TargetEnemy = _curTargetEnemy;
    }
    
    void InitCommonData()
    {
        AllBullets = GM.Root.InventoryMgr._BulletInvData.EquipBullets;
        if (GM.Root.BattleMgr.battleData.CurLevel == null)
            CurEnemy = null;
        else
            CurEnemy = GM.Root.BattleMgr.battleData.CurLevel.CurEnemy.Data;
        CurBullet = null;
        TargetEnemy = null;
        // 默认房间数据
        EnterRoomID = -1;
        IsFirstEnterRoom = false;
        ShieldSkipCount = false;
    }
}

public enum ItemTriggerTiming
{
    OnAlltimes = 0,
    OnBattleStart = 1,
    OnBulletFire = 2,
    OnBulletHit = 3,
    OnShieldPenetrate = 4,
    OnEnterRoom = 5,
    Passive = 6,
}
#endregion