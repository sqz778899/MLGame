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
    public List<string> TagAffinity = new(); // 亲和Tag（比如Bone、Magic）
    public float AffinityWeightMultiplier; // 命中一个Tag后权重增加

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

public static class DropTableService
{
    static Dictionary<string, List<DropedObjEntry>> _dropTables = new();
    static HashSet<int> _drawnUniqueItems = new();
    
    public static void LoadFromJson()
    {
        // 解析 JSON 数据
        _dropTables.Clear();
        List<DropTableJson> dropTableJson = TrunkManager.Instance.GetDropTableJson();
        foreach (DropTableJson each in dropTableJson)
        {
            each.Entries.ForEach(d=>d.InitData());
            _dropTables[each.PoolName] = each.Entries;
        }
    }
    
    public static DropedObjEntry Draw(string poolName,int _mapNodeDataID,List<string> clutterTags = null)
    {
        if (!_dropTables.TryGetValue(poolName, out var entries) || entries.Count == 0)
        {
            Debug.LogError($"DropTableService: 未找到掉落池 {poolName}");
            return null;
        }
        
        //Step1.过滤不可重复项 （道具不能抽重复的，这里过滤一下池子）
        List<DropedObjEntry> filtered = entries
            .Where(e => !e.OnlyOncePerRun || !_drawnUniqueItems.Contains(e.ID))
            .ToList();
        if (filtered.Count == 0)
        {
            Debug.LogWarning($"DropTableService: 所有可抽元素均已被抽取，池 {poolName} 为空");
            return null;
        }
        
        //Step2,根据抽取物Tag调整权重
        Dictionary<DropedObjEntry, int> adjusted = clutterTags == null
            ? filtered.ToDictionary(e => e, e => e.Weight)
            : DropPoolWeightAdjuster.AdjustWeights(filtered, clutterTags);

        Dictionary<string, int> weightMap = adjusted.ToDictionary(
            e => $"{e.Key.ID}_{e.Key.DropedCategory}", e => e.Value);
        
        //Step3，抽取，拿到唯一Key
        int mapRandomSeed = GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed;
        int localSeed = mapRandomSeed + _mapNodeDataID * 17000; // 真正扰动 seed！
        string bucketKey = $"gambling_{mapRandomSeed}_{_mapNodeDataID * 17000}_{poolName}";//通过 mapID 和池名来区分伪随机池
        string selectedKey = ProbabilityService.Draw(bucketKey, weightMap, localSeed);
        
        //Step4，反解析 key ，找回原始 entry
        string[] parts = selectedKey.Split('_');
        int selectedID = int.Parse(parts[0]);
        DropedCategory selectedCategory = Enum.Parse<DropedCategory>(parts[1]);
        DropedObjEntry selectedEntry = filtered.FirstOrDefault(
            e => e.ID == selectedID && e.DropedCategory == selectedCategory);
        if (selectedEntry == null)
        {
            Debug.LogError($"DropTableService: 无法根据 key 找到对应的 Entry：{selectedKey}");
            return null;
        }
        //Step5，如果是不可重复项（道具）的话，添加到查重池
        if (selectedEntry.OnlyOncePerRun)
            _drawnUniqueItems.Add(selectedEntry.ID);

        return selectedEntry;
    }
    
    public static void ResetAll()
    {
        _dropTables.Clear();
        _drawnUniqueItems.Clear();
    }
}

//展示物品奖励的封装
public static class DropRewardService
{
    public static void Drop(DropedObjEntry drop, Vector3 startPos)
    {
        if (drop == null)
        {
            Debug.LogError("DropRewardService: 尝试处理空的掉落对象！");
            return;
        }
        
        // Step1：触发特效（光团飞向背包）
        //FlipRewardFxManager.Instance.PlayDropEffect(drop);
        EParameter para = new EParameter
        {
            CurEffectType = EffectType.FlipReward,
            InsNum = 1,
            StartPos = startPos,
            Radius = 2f,
            SpawntimeRange = new Vector2(0.3f,0.4f),
            FlyRangeOffset  = new Vector2(0.5f,0.8f),
            FlyTimeRange = new Vector2(0.5f,0.8f),
        };

        EffectManager effectManager = EternalCavans.Instance._EffectManager;
        effectManager.CreatEffect(para, false, () =>
        {
            RewardBannerManager.Instance.ShowReward(drop.Icon, 1, drop.Rarity);
        });

        // 加到背包
        switch (drop.DropedCategory)
        {
            case DropedCategory.Gem:
                InventoryManager.Instance.AddGemToBag(drop.ID);
                break;
            case DropedCategory.Item:
                InventoryManager.Instance.AddItemToBag(drop.ID);
                break;
            default:
                Debug.LogWarning($"DropRewardService: 未知掉落类型 {drop.DropedCategory}");
                break;
        }

        // 展示奖励Banner
        //RewardBannerManager.Instance.ShowReward(drop.Icon, 1, drop.Rarity);
    }
}


public static class DropPoolWeightAdjuster
{
    public static Dictionary<DropedObjEntry, int> AdjustWeights(
        List<DropedObjEntry> entries, List<string> clutterTags)
    {
        Dictionary<DropedObjEntry, int> adjusted = new();

        foreach (var entry in entries)
        {
            int finalWeight = entry.Weight;
            float affinityBonusRate = entry.AffinityWeightMultiplier > 0f ? entry.AffinityWeightMultiplier : 0.5f;

            if (entry.TagAffinity != null && entry.TagAffinity.Count > 0)
            {
                int matchCount = entry.TagAffinity.Count(tag => clutterTags.Contains(tag));
                if (matchCount > 0)
                {
                    finalWeight = Mathf.RoundToInt(finalWeight * (1f + affinityBonusRate * matchCount));
                }
            }

            adjusted[entry] = finalWeight;
        }

        return adjusted;
    }
}
