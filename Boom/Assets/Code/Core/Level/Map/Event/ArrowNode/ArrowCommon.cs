using UnityEngine;

public interface IArrowStrategy
{
    void Execute(MapNodeData data, MapNodeView view);
}

//子类别（RoomArrow）
public enum RoomArrowType
{
    Normal = 0,
    Fight = 1,
    KeyGate = 2,
    ReturnStone = 3
}


//Normal 跳房间策略
public class RoomArrowNormalStrategy : IArrowStrategy
{
    public void Execute(MapNodeData data, MapNodeView view)
    {
        RoomArrowRuntimeData runtime = data.EventData as RoomArrowRuntimeData;
        MapManager mapMgr = BattleManager.Instance._MapManager;
        mapMgr.CurMapSate.TargetRoomID = runtime.TargetRoomID;
        mapMgr.ToTargetRoom();  
        view.SetAsTriggered();
    }
}

//Fight 战斗策略
public class RoomArrowFightStrategy : IArrowStrategy
{
    public void Execute(MapNodeData data, MapNodeView view)
    {
        var runtime = data.EventData as RoomArrowRuntimeData;
        if (runtime.BattleConfig == null) { Debug.LogError("战斗敌人配置缺失"); return; }
        
        DialogueFight diaFightSC = EternalCavans.Instance.DialogueFightSC;
        diaFightSC.gameObject.SetActive(true);
        diaFightSC.InitData(data);
        //预设好目标房间
        BattleManager.Instance._MapManager.CurMapSate.TargetRoomID = runtime.TargetRoomID;
        UIManager.Instance.IsLockedClick = true;
        data.IsLocked = true;
    }
}

//KeyGate 需要钥匙策略
public class RoomArrowKeyGateStrategy : IArrowStrategy
{
    public void Execute(MapNodeData data, MapNodeView view)
    {
        var runtime = data.EventData as RoomArrowRuntimeData;
        var player = GM.Root.PlayerMgr._PlayerData;

        if (player.RoomKeys <= 0 && !data.IsTriggered)
        {
            view.ShowFloatingText("锁上了");
            FloatingTextFactory.CreateWorldText("我需要一把钥匙", default,
                FloatingTextType.MapHint,Color.yellow, 2f);
            return;
        }

        if (!data.IsTriggered)
        {
            player.ModifyRoomKeys(-1);
            data.IsTriggered = true;
            view.SetAsTriggered();
        }
        MapManager mapMgr = BattleManager.Instance._MapManager;
        mapMgr.CurMapSate.TargetRoomID = runtime.TargetRoomID;
        mapMgr.ToTargetRoom();  
    }
}

//ReturnStone 结算回城策略
public class RoomArrowReturnStoneStrategy : IArrowStrategy
{
    public void Execute(MapNodeData data, MapNodeView view) 
        => EternalCavans.Instance.ShowConquerTheLevelGUI();
}