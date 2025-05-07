using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//伪随机赌博类
public class BasicGamblingEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        BasicGamblingRuntimeData gamblingData = data.EventData as BasicGamblingRuntimeData;
        if (gamblingData == null) return;
        //Step1：构建大池子
        Dictionary<string, int> weights = BuildGamblingWeights(gamblingData);
        //Step2：抽取大池子
        int rollCount  = Random.Range(gamblingData.MinDropCount, gamblingData.MaxDropCount + 1);
        List<string> rolledResults = RollBigPool(data, rollCount, weights);
        //Step3：协程播放掉落
        view.StartCoroutine(PlayRewardSequence(rolledResults, data, view));
    }

    IEnumerator PlayRewardSequence(List<string> resultList, MapNodeData data, MapNodeView view)
    {
        List<DropedObjEntry> collectedDrops = new(); // 收集真实掉落
        foreach (string eachResult in resultList)
        {
            switch (eachResult)
            {
                case "EmptyChance":
                    view.ShowFloatingText("空空如也…");
                    break;
                case "KeyChance":
                    view.ShowFloatingText("你找到了一把钥匙！");
                    PlayKeyReward(view.transform.position);
                    GM.Root.PlayerMgr._PlayerData.ModifyRoomKeys(1);
                    break;
                case "BuffChance":
                case "DebuffChance":
                case "NormalLoot":
                case "MetaResource":
                case "RareLoot":
                    DropedObjEntry drop = DropTableService.Draw(eachResult, data.ID, data.ClutterTags);
                    if (drop != null)
                        collectedDrops.Add(drop);
                    break;
            }
        }
        
        List<(DropedObjEntry drop, int count)> mergedDrops = DropTableService.MergeDrops(collectedDrops);
            
        foreach (var (drop, count) in mergedDrops)
        {
            if (drop.DropedCategory == DropedCategory.Buff)
            {
                for (int i = 0; i < count; i++)
                   GM.Root.PlayerMgr._PlayerData.AddBuff(drop.ID); // ID就是Buff ID
                
                // 播放获得buff提示（可选）
                FloatingTextFactory.CreateWorldText($"获得新的效果：{drop.ID}", 
                    view.transform.position + Vector3.up * 1.5f,
                    FloatingTextType.MapHint, Color.cyan, 2f);
            }
            else
                DropRewardService.Drop(drop, view.transform.position, data.EventType,count);
            yield return new WaitForSeconds(0.25f); // 每个掉落之后等一下
        }
        
        // 道具 翻找术手册 有50%概率可以额外翻找一次
        bool allowExtra = GM.Root.InventoryMgr.MiracleOddityMrg.ShouldRetryGambling();
        if (data.SearchCount == 0 && allowExtra)
        {
            FloatingTextFactory.CreateWorldText($"触发翻找术手册！！", default,
                FloatingTextType.MapHint, Color.green, 2.5f);
            // 允许再翻一次，不标记 IsTriggered
            data.SearchCount++; // 标记本次是额外翻找
        }
        else
        {
            data.SearchCount++;
            data.IsTriggered = true; // 结束翻找流程
        }
        view.SetAsTriggered();
    }

    #region 不关心的私有方法
    Dictionary<string, int> BuildGamblingWeights(BasicGamblingRuntimeData data)
    {
        return new()
        {
            { "EmptyChance", data.EmptyChance },
            { "KeyChance", data.KeyChance },
            { "BuffChance", data.TempBuffChance },
            { "DebuffChance", data.TempDebuffChance },
            { "NormalLoot", data.NormalLootChance },
            { "MetaResource", data.MetaResourceChance },
            { "RareLoot", data.RareLootChance }
        };
    }
    
    List<string> RollBigPool(MapNodeData data, int rollCount, Dictionary<string, int> weights)
    {
        List<string> results = new();
        int baseSeed = GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed;
        string bucketKey = $"bigPool_{data.ID * 17000}";

        for (int i = 0; i < rollCount; i++)
        {
            int localSeed = baseSeed + data.ID * 17000 + i * 2333;
            string result = ProbabilityService.Draw(bucketKey, weights, localSeed);
            results.Add(result);
        }

        return results;
    }
    
    void PlayKeyReward(Vector3 startPos)
    {
        var para = new EParameter
        {
            CurEffectType = EffectType.RoomKeys,
            StartPos = startPos
        };

        EternalCavans.Instance._EffectManager.CreatEffect(para, null, () =>
        {
            FloatingTextFactory.CreateWorldText("获得一个钥匙！", default, FloatingTextType.MapHint, Color.yellow, 2f);
        });
    }
    #endregion
}