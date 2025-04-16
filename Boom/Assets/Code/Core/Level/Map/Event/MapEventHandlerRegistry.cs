using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMapEventHandler
{
    void Handle(MapNodeData data, MapNodeView view);
}

public static class MapEventHandlerRegistry
{
    private static readonly Dictionary<MapEventType, IMapEventHandler> _handlers = new();

    static MapEventHandlerRegistry()
    {
        // 注册各类事件处理器（只初始化一次）
        Register(MapEventType.CoinsPile, new CoinsPileEventHandler());
        Register(MapEventType.WeaponRack, new WeaponRackEventHandler());
        Register(MapEventType.Skeleton, new SkeletonHandler());
        Register(MapEventType.StoneTablet,new StoneTabletHandler());
        Register(MapEventType.MysticalInteraction,new WigglingBoxHandler());
        // 更多事件类型...
    }

    public static void Register(MapEventType type, IMapEventHandler handler) => _handlers[type] = handler;

    public static IMapEventHandler GetHandler(MapEventType type)
    {
        return _handlers.TryGetValue(type, out var handler) ? handler : null;
    }
}

#region 奖励类
public class CoinsPileEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        CoinsPileRuntimeData coinsData = data.EventData as CoinsPileRuntimeData;

        int amount = Random.Range(coinsData.MinGold, coinsData.MaxGold + 1);
        PlayerManager.Instance._PlayerData.ModifyCoins(amount);
        view.ShowFloatingText($"获得 {amount} 枚金币！");
        view.SetAsTriggered(amount); //把数量传回 View
    }
}
#endregion

#region 伪随机赌博类
public class WeaponRackEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        var weaponData = data.EventData as WeaponRackRuntimeData;
        if (weaponData == null) return;

        Dictionary<string, int> weights = new()
        {
            { "Empty", weaponData.EmptyChance },
            { "TempBuff", weaponData.TempBuffChance },
            { "TempDebuff", weaponData.TempDebuffChance },
            { "NormalLoot", weaponData.NormalLootChance },
            { "Meta", weaponData.MetaResourceChance },
            { "Rare", weaponData.RareLootChance }
        };

        string result = ProbabilityService.Draw($"weaponrack_{data.ID}", weights,
            GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed);

        switch (result)
        {
            case "Empty": view.ShowFloatingText("空空如也…"); break;
            case "TempBuff": view.ShowFloatingText("获得临时 Buff！"); break;
            case "TempDebuff": view.ShowFloatingText("诶呀，有点不对劲…"); break;
            case "NormalLoot": view.ShowFloatingText("捡到一把普通的破武器。"); break;
            case "Meta": view.ShowFloatingText("发现稀有材料：秘银！"); break;
            case "Rare": view.ShowFloatingText("这是传说中的…神器？"); break;
        }
    }
}

public class SkeletonHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        if (data.EventData is not SkeletonRuntimeData skeletonData)
        {
            Debug.LogWarning("Skeleton事件缺少有效数据");
            return;
        }

        var weights = new Dictionary<string, int>
        {
            { "Empty", skeletonData.EmptyChance },
            { "Note", skeletonData.NoteChance },
            { "Item", skeletonData.ItemChance },
            { "Debuff", skeletonData.DebuffChance },
            { "Key", skeletonData.KeyChance }
        };

        string result = ProbabilityService.Draw($"Skeleton_Layer_{data.ID}", weights,
            GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed);

        switch (result)
        {
            case "Empty":
                view.ShowFloatingText("只剩一堆骨头…");
                break;
            case "Note":
                view.ShowFloatingText($"发现了一张纸条：");
                break;
            case "Item":
                // 假设掉一个金币
                PlayerManager.Instance._PlayerData.ModifyCoins(1);
                view.ShowFloatingText("找到了一枚生锈的金币");
                break;
            case "Debuff":
                // 示例逻辑：下场战斗第一颗子弹 -1 穿透
                view.ShowFloatingText("你感到一阵阴冷…");
                break;
            case "Key":
                view.ShowFloatingText("从骨缝中找到了一把腐朽的钥匙！");
                break;
        }

        data.IsTriggered = true;
        view.SetAsTriggered();
    }
}
#endregion

#region Lore探索型
public class StoneTabletHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        if (data.EventData is not StoneTabletRuntimeData runtime)
        {
            Debug.LogWarning("StoneTablet数据为空");
            return;
        }

        if (runtime.LoreTexts == null || runtime.LoreTexts.Count == 0)
        {
            view.ShowFloatingText("石碑上的文字已模糊不清…");
        }
        else
        {
            string line = runtime.LoreTexts[UnityEngine.Random.Range(0, runtime.LoreTexts.Count)];
            view.ShowFloatingText($"石碑刻着：{line}");

            // 可选推送到日志系统
            // LogbookSystem.RecordLore(line);
        }

        data.IsTriggered = true;
        view.SetAsTriggered();
    }
}
#endregion

#region 奇异互动型
public class WigglingBoxHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        if (data.EventData is not WigglingBoxRuntimeData runtime)
        {
            Debug.LogWarning("WigglingBox数据为空");
            return;
        }

        var weights = new Dictionary<string, int>
        {
            { "Run", runtime.RunAwayChance },
            { "Talk", runtime.TalkChance },
            { "Boom", runtime.ExplosionChance },
            { "Loot", runtime.LootChance }
        };

        string result = ProbabilityService.Draw($"WigglingBox_{data.ID}", weights,
            GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed);

        switch (result)
        {
            case "Run":
                view.ShowFloatingText("它跳起来逃走了！");
                view.StartCoroutine(BoxHopAway(view)); // 箱子跳走
                break;
            case "Talk":
                view.ShowFloatingText("箱子低语道：不要再来了……");
                break;
            case "Boom":
                view.ShowFloatingText("砰！箱子爆炸了！");
                // 可以播放粒子特效、推送 Debuff 等
                break;
            case "Loot":
                view.ShowFloatingText("你捡到了一枚金币！");
                PlayerManager.Instance._PlayerData.ModifyCoins(1);
                break;
        }

        data.IsTriggered = true;
        view.SetAsTriggered();
    }

    IEnumerator BoxHopAway(MapNodeView view)
    {
        Vector3 start = view.transform.position;
        Vector3 end = start + Vector3.right * UnityEngine.Random.Range(1f, 2f);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            view.transform.position = Vector3.Lerp(start, end, t) + Vector3.up * Mathf.Sin(t * Mathf.PI);
            yield return null;
        }

        view.gameObject.SetActive(false);
    }
}

#endregion


