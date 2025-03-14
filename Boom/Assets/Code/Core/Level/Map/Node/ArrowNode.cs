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
    
    internal override void OnMouseEnter()
    {
        spriteRenderer.color = HeighLightColor;// 将精灵高亮显示
        if (Input.GetMouseButtonDown(0))
            spriteRenderer.transform.localScale = defaultScale * 0.8f;
        if (Input.GetMouseButtonUp(0))
            spriteRenderer.transform.localScale = defaultScale;
    }

    internal override void OnMouseExit() => spriteRenderer.color = defaultColor;
    
    //去下一个房间
    public void GoToOtherNode()
    {
        if (TutorialLocked)
        {
            FloatingText("需要解锁横向房间",new Color(1,1,1,1f));
            return;
        }
        if (IsLocked) return;
        MainRoleManager.Instance.CurMapSate.TargetRoomID = TargetRoom.RoomID;
        GameObject dialogueFightIns = ResManager.instance.CreatInstance(PathConfig.DialogueFightPB);
        dialogueFightIns.transform.SetParent(UIManager.Instance.DialogueRoot.transform,false);
        dialogueFightIns.GetComponent<DialogueFight>().InitData(this);
        IsLocked = true;
        UIManager.Instance.IsLockedClick = true;
    }
    
    public void GoToLockedRoom()
    {
        if (IsLocked) return;
        MainRoleManager.Instance.CurMapSate.CurRoomID = TargetRoom.RoomID;
        MMapManager.SetRolePos();
    }
    
    public void GoToLockedRoomWithKey()
    {
        if (IsLocked) return;
        if (MainRoleManager.Instance.RoomKeys == 0)
        {
            FloatingText("锁上了");
            FloatingGetItemText("我需要一把钥匙");
            return;
        }
        MainRoleManager.Instance.RoomKeys -= 1;
        
        ArrowUnlocked.SetActive(true);
        MainRoleManager.Instance.CurMapSate.CurRoomID = TargetRoom.RoomID;
        MMapManager.SetRolePos();
        gameObject.SetActive(false);
    }
}
