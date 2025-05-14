using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DropedObjEntry
{
    public int ID; // 掉落物ID
    public string Name;
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
                Name = itemjson.NameKey;
                break;
            case DropedCategory.Gem:
                GemJson gemjson = TrunkManager.Instance.GetGemJson(ID);
                Icon = ResManager.instance.GetGemIcon(ID);
                Rarity = gemjson.Rarity;
                Name = gemjson.NameKey;
                break;
            case DropedCategory.MiracleOddity:
                MiracleOddityJson mojson = TrunkManager.Instance.GetMiracleOddityJson(ID);
                Icon = ResManager.instance.GetMiracleOddityIcon(ID);
                Rarity = mojson.Rarity;
                Name = mojson.NameKey;
                break;
            case DropedCategory.Buff:
                BuffJson buffjson = TrunkManager.Instance.GetBuffJson(ID);
                Icon = ResManager.instance.GetBuffIcon(ID);
                Name = buffjson.Name;
                Rarity = buffjson.Rarity;
                break;
        }
    }
}

public static class DropTableService
{
    static Dictionary<string, List<DropedObjEntry>> _dropTables = new();
    
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
        if (!_dropTables.TryGetValue(poolName, out List<DropedObjEntry> entries) || entries.Count == 0)
        {
            Debug.LogError($"DropTableService: 未找到掉落池 {poolName}");
            return null;
        }
        
        //Step1.过滤不可重复项 （道具不能抽重复的，这里过滤一下池子）
        List<DropedObjEntry> filtered = new List<DropedObjEntry>();
        foreach (DropedObjEntry eachDropedEntry in entries)
        {
            if (eachDropedEntry.DropedCategory == DropedCategory.MiracleOddity)
            {
                if (!GM.Root.InventoryMgr.MiracleOdditiesDuplicateCheck(eachDropedEntry.ID))
                    filtered.Add(eachDropedEntry);
            }
            else
                filtered.Add(eachDropedEntry);
        }
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
        return selectedEntry;
    }
    
    #region 合并掉落工具
    public static List<(DropedObjEntry drop, int count)> MergeDrops(List<DropedObjEntry> originalDrops)
    {
        Dictionary<(int id, DropedCategory cat), int> counter = new();
        Dictionary<(int id, DropedCategory cat), DropedObjEntry> keeper = new();

        foreach (var drop in originalDrops)
        {
            var key = (drop.ID, drop.DropedCategory);
            if (!counter.ContainsKey(key))
            {
                counter[key] = 0;
                keeper[key] = drop;
            }
            counter[key]++;
        }

        List<(DropedObjEntry drop, int count)> merged = new();
        foreach (var kvp in counter)
        {
            merged.Add((keeper[kvp.Key], kvp.Value));
        }

        return merged;
    }
    #endregion
    
    public static void ResetAll() => _dropTables.Clear();
}

//展示物品奖励的封装
public static class DropRewardService
{
    public static void Drop(DropedObjEntry drop, Vector3 startPos,MapEventType eventType,int count)
    {
        if (drop == null)
        {
            Debug.LogError("DropRewardService: 尝试处理空的掉落对象！");
            return;
        }
        drop.InitData();
        
        // Step1：根据稀有度设置不同的特效参数
        EParameter para = GetParaByRarity(drop.Rarity, startPos);
        // Step2：根据事件类型设置不同的特效参数
        para = SetParaByEventType(para, eventType);
        EffectManager effectManager = EternalCavans.Instance._EffectManager;
        // Step3：触发特效（光团飞向背包）
        effectManager.CreatEffect(para, null, () =>
        {
            RewardBannerManager.Instance.ShowReward(drop, count);
        });

        // Step4：数据加到背包
        switch (drop.DropedCategory)
        {
            case DropedCategory.Gem:
                InventoryManager.Instance.AddGemToBag(drop.ID,count);
                break;
            case DropedCategory.Item:
                InventoryManager.Instance.AddItemToBag(drop.ID,count);
                break;
            case DropedCategory.MiracleOddity:
                InventoryManager.Instance.EquipMiracleOddity(drop.ID);
                break;
            default:
                Debug.LogWarning($"DropRewardService: 未知掉落类型 {drop.DropedCategory}");
                break;
        }
    }

    #region 根据稀有度&&事件类型，设置特效参数
    static EParameter GetParaByRarity(DropedRarity rarity, Vector3 startPos)
    {
        EParameter para = new EParameter
        {
            CurEffectType = EffectType.FlipReward,
            StartPos = startPos,
        };

        switch (rarity)
        {
            case DropedRarity.Common:
                para.InsNum = 6;
                para.Radius = 1.0f;
                para.SpawntimeRange = new Vector2(0.2f, 0.3f);
                para.FlyRangeOffset = new Vector2(0.5f, 0.8f);
                para.FlyTimeBase = 0.2f;
                para.FlyTimePerUnitDistance = 0.04f;
                para.FlyTimeClampMax = 0.6f;
                para.SpecialEffectPath = PathConfig.AwardTraitCommon; // 普通光球
                break;
            case DropedRarity.Rare:
                para.InsNum = 12;
                para.Radius = 1.5f;
                para.SpawntimeRange = new Vector2(0.25f, 0.35f);
                para.FlyRangeOffset = new Vector2(0.6f, 0.9f);
                para.FlyTimeBase = 0.25f;
                para.FlyTimePerUnitDistance = 0.045f;
                para.FlyTimeClampMax = 0.7f;
                para.SpecialEffectPath = PathConfig.AwardTraitRare; // 带亮光
                break;
            case DropedRarity.Epic:
                para.InsNum = 20;
                para.Radius = 2.0f;
                para.SpawntimeRange = new Vector2(0.35f, 0.45f);
                para.FlyRangeOffset = new Vector2(0.7f, 1.0f);
                para.FlyTimeBase = 0.3f;
                para.FlyTimePerUnitDistance = 0.05f;
                para.FlyTimeClampMax = 0.8f;
                para.SpecialEffectPath = PathConfig.AwardTraitEpic; // 带拖尾
                break;
            case DropedRarity.Legendary:
                para.InsNum = 30;
                para.Radius = 2.5f;
                para.SpawntimeRange = new Vector2(0.5f, 0.6f);
                para.FlyRangeOffset = new Vector2(1.0f, 1.2f);
                para.FlyTimeBase = 0.4f;
                para.FlyTimePerUnitDistance = 0.06f;
                para.FlyTimeClampMax = 1.0f;
                para.SpecialEffectPath = PathConfig.AwardTraitLegendary; // 彩虹爆炸粒子
                break;
        }
        return para;
    }

    static EParameter SetParaByEventType(EParameter para, MapEventType eventType)
    {
        switch (eventType)
        {
            case MapEventType.TreasureBox:
                para.ExplodeMode = EffectExplodeMode.Upward;
                break;
            case MapEventType.BasicGambling:
                para.ExplodeMode = EffectExplodeMode.Sphere;
                break;
        }

        return para;
    }
    #endregion
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
