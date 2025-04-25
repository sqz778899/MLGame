using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DropedObjEntry
{
    public int ID; // 掉落物ID
    public int Weight; // 权重
    public DropedCategory DropedCategory; // 是宝石还是道具
    public Sprite Icon;
    public DropedRarity Rarity;
    public bool OnlyOncePerRun = false; // 默认道具重复，特殊标记为唯一

    public void InitData()
    {
        switch (DropedCategory)
        {
            case DropedCategory.Item:
                ItemJson itemjson = TrunkManager.Instance.GetItemJson(ID);
                Icon = ResManager.instance.GetItemIcon(ID);
                Rarity = itemjson.Rarity;
                break;
            case DropedCategory.Gem:
                GemJson gemjson = TrunkManager.Instance.GetGemJson(ID);
                Icon = ResManager.instance.GetGemIcon(ID);
                Rarity = gemjson.Rarity;
                break;
        }
    }
}

public class DropTableService
{
    static HashSet<int> _drawnUniqueItems = new();
    
    public static DropedObjEntry Draw(string poolName, int seed)
    {
        DropTableJson dropTableJson = TrunkManager.Instance.GetDropTableJson(poolName);
        if (dropTableJson == null )
        {
            Debug.LogError($"DropTableService: 未找到掉落池 {poolName}");
            return null;
        }
        
        List<DropedObjEntry> entries = TrunkManager.Instance.GetDropTableJson(poolName).Entries;
        
        //道具不能抽重复的，这里过滤一下池子
        List<DropedObjEntry> filtered = entries
            .Where(e => !e.OnlyOncePerRun || !_drawnUniqueItems.Contains(e.ID))
            .ToList();

        if (filtered.Count == 0)
        {
            Debug.LogWarning($"DropTableService: 所有可抽元素均已被抽取，池 {poolName} 为空");
            return null;
        }

        Dictionary<string, int> weights = filtered.ToDictionary(
            e => $"{e.ID}_{e.DropedCategory}", e => e.Weight);
        // 拿到抽取到的唯一Key
        string selectedKey = ProbabilityService.Draw($"droppool_{poolName}", weights, seed);
        
        // 解析 key 回来
        string[] parts = selectedKey.Split('_');
        int selectedID = int.Parse(parts[0]);
        DropedCategory selectedCategory = Enum.Parse<DropedCategory>(parts[1]);

        // 找回原始 entry
        DropedObjEntry selectedEntry = filtered.FirstOrDefault(
            e => e.ID == selectedID && e.DropedCategory == selectedCategory);
        if (selectedEntry == null)
        {
            Debug.LogError($"DropTableService: 无法根据 key 找到对应的 Entry：{selectedKey}");
            return null;
        }
        //如果是道具的话，添加到查重池
        if (selectedEntry.OnlyOncePerRun)
            _drawnUniqueItems.Add(selectedEntry.ID);

        return selectedEntry;
    }
    
    public static void ResetOncePerRunRecord() => _drawnUniqueItems.Clear();
}