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
    
    void OnClutterSearchResolved();//地图类特质触发
    
    void RemoveEffect();
    MiracleOddityTriggerTiming TriggerTiming { get; }
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

    public IMiracleOddityEffect EffectLogic; // 每个道具一个策略实现

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
    public bool IsStackable => PersistentType == PersistentItemType.Resource;
    #endregion

    public ItemData(int id,ItemSlotController itemSlotController)
    {
        var json = TrunkManager.Instance.GetItemJson(id);
        CurSlotController = itemSlotController;
        ID = json.ID;
        Name = json.Name;
        Desc = json.Desc;
        Rarity = json.Rarity;
        PersistentType = json.PersistentType;
        ImageName = json.ResName;
        EffectLogic = MiracleOddityEffectFactory.CreateEffectLogic(ID);
        
        // 新加的
        //MaxStackCount = json.MaxStackCount > 0 ? json.MaxStackCount : 1; // 读配置
        MaxStackCount = 5;
        StackCount = 1;
    }
    
    public ToolTipsInfo BuildTooltip()
    {
        ToolTipsInfo info = new ToolTipsInfo(Name,Level,Desc, ToolTipsType.Item,
            Rarity,PersistentType);
        return info;
    }
}
#endregion