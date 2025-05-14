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
    public string Desc;
    public DropedRarity Rarity;
    ItemJson _json => TrunkManager.Instance.GetItemJson(ID);
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
        ItemJson json = TrunkManager.Instance.GetItemJson(id);
        CurSlotController = itemSlotController;
        ID = json.ID;
        Rarity = json.Rarity;
        PersistentType = json.PersistentType;
        MaxStackCount = 5;
        StackCount = 1;
        Loc.OnLanguageChanged -= SyncStrInfo;
        Loc.OnLanguageChanged += SyncStrInfo;//语言改变事件
        SyncStrInfo(json);
    }
    
    #region 处理本地化多语言相关
    void SyncStrInfo() => SyncStrInfo(_json);
    void SyncStrInfo(ItemJson json)
    {
        Name = Loc.Get(json.NameKey);
        Desc = Loc.Get(json.DescKey);
    }
    ~ItemData() => ClearData();
    public void ClearData() => Loc.OnLanguageChanged -= SyncStrInfo;
    #endregion
    
    public ToolTipsInfo BuildTooltip() => new ToolTipsInfo(Name,Level,
        Desc,ToolTipsType.Item, Rarity,PersistentType);
}
#endregion