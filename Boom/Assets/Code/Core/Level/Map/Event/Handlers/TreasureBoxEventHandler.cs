using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//宝箱事件
public class TreasureBoxEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        if (data.EventData is not TreasureBoxRuntimeData chestData)
        {
            Debug.LogWarning("宝箱事件数据为空！");
            return;
        }
        //Step1 Roll数量
        int rollCount = Random.Range(chestData.MinLootCount, chestData.MaxLootCount + 1);
        //Step2 Roll真实的掉落
        List<DropedObjEntry> rolledDrops = RollDrops(data, rollCount);
        //Step3 开启协程依次展示掉落
        view.StartCoroutine(PlayDropsSequentially(view, rolledDrops));
        //Step4 回调下通用触发
        data.IsTriggered = true;
        view.SetAsTriggered();
    }
    
    IEnumerator PlayDropsSequentially(MapNodeView view, List<DropedObjEntry> drops)
    {
        List<(DropedObjEntry drop, int count)> mergedDrops = DropTableService.MergeDrops(drops);
        foreach (var (drop, count) in mergedDrops)
        {
            DropRewardService.Drop(drop, view.transform.position, MapEventType.TreasureBox, count);
            yield return new WaitForSeconds(0.3f); //等待一小段时间后再掉下一个 可以调节节奏快慢
        }
    }

    List<DropedObjEntry> RollDrops(MapNodeData data, int count)
    {
        TreasureBoxRuntimeData chestData = data.EventData as TreasureBoxRuntimeData;
        List<DropedObjEntry> table = chestData.DropTable; //找到配置表
        // 构造 key：每个宝箱的伪随机 key 应该唯一
        string bucketKey = $"Chest:{data.ID}"; // 假设每个 MapNodeData 有唯一 ID

        // 构建概率字典：把 DropedObjEntry 转成 Dictionary<string, int>
        Dictionary<string, int> weightDict = new();
        Dictionary<string, DropedObjEntry> idToEntry = new();

        foreach (var entry in table)
        {
            string key = entry.ID.ToString();
            weightDict[key] = entry.Weight;
            idToEntry[key] = entry;
        }

        // 使用伪随机系统抽取 count 个物品
        List<DropedObjEntry> result = new();
        for (int i = 0; i < count; i++)
        {
            string rollResult = ProbabilityService.Draw(bucketKey, weightDict, 
                GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed);
            if (idToEntry.TryGetValue(rollResult, out var drop))
                result.Add(drop);
            else
                Debug.LogError($"掉落表中找不到ID为 {rollResult} 的物品");
        }
        return result;
    }
}