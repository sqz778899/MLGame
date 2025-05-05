using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Register(MapEventType.TreasureBox,new TreasureBoxEventHandler());
        Register(MapEventType.Bullet,new BulletEventHandler());
        Register(MapEventType.RoomKey, new RoomKeyEventHandler());
        Register(MapEventType.BasicGambling, new BasicGamblingEventHandler());
        Register(MapEventType.StoneTablet,new StoneTabletHandler());
        Register(MapEventType.MysticalInteraction,new WigglingBoxHandler());
        Register(MapEventType.Shop, new ShopEventHandler());
        
        Register(MapEventType.RoomArrow, new RoomArrowEventHandler());// 房间箭头
        // 更多事件类型...
    }

    public static void Register(MapEventType type, IMapEventHandler handler) => _handlers[type] = handler;

    public static IMapEventHandler GetHandler(MapEventType type) => 
        _handlers.TryGetValue(type, out var handler) ? handler : null;
}

#region 奖励类
//金币堆事件
public class CoinsPileEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        CoinsPileRuntimeData coinsData = data.EventData as CoinsPileRuntimeData;

        int amount = Random.Range(coinsData.MinGold, coinsData.MaxGold + 1);
        GM.Root.PlayerMgr._PlayerData.ModifyCoins(amount);
        view.ShowFloatingText($"获得 {amount} 枚金币！");
        view.SetAsTriggered(amount); //把数量传回 View
    }
}
//宝箱事件
//子弹事件
public class BulletEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        var runtime = data.EventData as BulletEventRuntimeData;
        if (runtime == null)
        {
            Debug.LogWarning("BulletEventRuntimeData 为空");
            return;
        }

        Dialogue dialogue = EternalCavans.Instance.DialogueSC;
        dialogue.LoadDialogue(runtime.DialogueName);
        dialogue.OnDialogueEnd += OnDialogueEnd;
        view.SetAsTriggered();

        void OnDialogueEnd()
        {
            dialogue.OnDialogueEnd -= OnDialogueEnd;
            InventoryManager.Instance._BulletInvData.AddSpawner(runtime.BulletID);

            BulletJson bulletDesignJson = TrunkManager.Instance.BulletDesignJsons
                .FirstOrDefault(b => b.ID == runtime.BulletID) ?? new BulletJson();
            FloatingTextFactory.CreateWorldText($"获得 {bulletDesignJson.Name}",
                default, FloatingTextType.MapHint,new Color(0.8f, 0.8f, 0.8f, 1), 3f);

            GameObject.Destroy(view.gameObject);
        }
    }
}
//钥匙事件
public class RoomKeyEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        var runtime = data.EventData as RoomKeyRuntimeData;
        if (runtime == null)
        {
            Debug.LogWarning("RoomKeyRuntimeData is null");
            return;
        }

        EParameter para = new EParameter {
            CurEffectType = EffectType.RoomKeys,
            StartPos = view.transform.position
        };
        EffectManager effectManager = EternalCavans.Instance._EffectManager;
        effectManager.CreatEffect(para,null, () =>
        {
            FloatingTextFactory.CreateWorldText("获得一个钥匙！", 
                default, FloatingTextType.MapHint,Color.yellow, 2f);
        });

        GM.Root.PlayerMgr._PlayerData.ModifyRoomKeys(runtime.RoomKeysNum);
        GameObject.Destroy(view.gameObject); // 直接移除地图物体
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
                GM.Root.PlayerMgr._PlayerData.ModifyCoins(1);
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
//商店事件
public class ShopEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        var runtime = data.EventData as ShopEventRuntimeData;
        if (runtime == null)
        {
            Debug.LogWarning("ShopEventRuntimeData 为空！");
            return;
        }

        // 第一次打开：创建并缓存商店
        if (runtime.ShopInstance == null)
        {
            GameObject shopIns = ResManager.instance.CreatInstance(PathConfig.ShopAsset);
            shopIns.transform.SetParent(EternalCavans.Instance.ShopRoot.transform, false);
            runtime.ShopInstance = shopIns;

            Shop shop = shopIns.GetComponent<Shop>();
            shop.InitData(runtime);
            shop.IsFirstOpen = true; // 默认第一次开启免费
        }
        view?.SetAsTriggered();
        // 每次点击都显示（但只创建一次）
        runtime.ShopInstance.GetComponent<ICloseOnClickOutside>()?.Show();
    }
}
#endregion

#region 房间箭头功能类
public class RoomArrowEventHandler : IMapEventHandler
{
    public void Handle(MapNodeData data, MapNodeView view)
    {
        if (data.EventData is not RoomArrowRuntimeData runtime)
        {
            Debug.LogWarning("RoomArrowRuntimeData 无效");
            return;
        }

        IArrowStrategy strategy = runtime.ArrowType switch
        {
            RoomArrowType.Normal => new RoomArrowNormalStrategy(),
            RoomArrowType.Fight => new RoomArrowFightStrategy(),
            RoomArrowType.KeyGate => new RoomArrowKeyGateStrategy(),
            RoomArrowType.ReturnStone => new RoomArrowReturnStoneStrategy(),
            _ => null
        };

        strategy?.Execute(data, view);
    }
}
#endregion
