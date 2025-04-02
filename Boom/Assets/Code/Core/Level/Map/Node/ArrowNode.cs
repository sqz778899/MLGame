using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowNode : MapNodeBase
{
    [Header("重要属性")]
    public Enemy CurEnemy;
    public MapRoomNode TargetRoom;

    [Header("横向房间专用")]
    public GameObject ArrowUnlocked;
    public bool TutorialLocked = false; //新手教程用
    
    MapRoomNode _curRoom;
    MapRoomNode curRoom
    {
        get
        {
            if (!_curRoom)
                _curRoom = transform.parent.parent.GetComponent<MapRoomNode>();
            return _curRoom;
        }
    }
    
    //去下一个房间
    public void GoToOtherNode()
    {
        if (TutorialLocked)
        {
            FloatingText("需要解锁横向房间",new Color(1,1,1,1f));
            return;
        }
        if (IsLocked) return;
        BattleManager.Instance._MapManager.CurMapSate.TargetRoomID = TargetRoom.RoomID;
        GameObject dialogueFightIns = ResManager.instance.CreatInstance(PathConfig.DialogueFightPB);
        dialogueFightIns.transform.SetParent(UIManager.Instance.CommonUI.DialogueRoot.transform,false);
        dialogueFightIns.GetComponent<DialogueFight>().InitData(this);
        IsLocked = true;
        UIManager.Instance.IsLockedClick = true;
    }
    
    //开完锁之后，普通的来回穿梭
    public void GoToLockedRoom()
    {
        if (IsLocked) return;
        BattleManager.Instance._MapManager.CurMapSate.SetCurRoomID(TargetRoom.RoomID);
        UIManager.Instance.Logic.MapManagerSC.SetRolePos();
    }
    
    //消耗钥匙去下一个房间
    public void GoToLockedRoomWithKey()
    {
        if (IsLocked) return;
        if (PlayerManager.Instance._PlayerData.RoomKeys == 0)
        {
            FloatingText("锁上了");
            FloatingGetItemText("我需要一把钥匙");
            return;
        }
        PlayerManager.Instance._PlayerData.ModifyRoomKeys(-1);
        
        ArrowUnlocked.SetActive(true);
        BattleManager.Instance._MapManager.CurMapSate.SetCurRoomID(TargetRoom.RoomID);
        UIManager.Instance.Logic.MapManagerSC.SetRolePos();
        gameObject.SetActive(false);
    }
    
    //游戏胜利返回城镇
    public void ReturnTown()
    {
        EternalCavans.Instance.ShowConquerTheLevelGUI();
    }
}
