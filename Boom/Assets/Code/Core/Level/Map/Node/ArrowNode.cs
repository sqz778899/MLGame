using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowNode : SpriteClickHandler
{
    [Header("重要属性")]
    public MapRoomNode mapRoomNode;

    public void GoToOtherNode()
    {
        MainRoleManager.Instance.CurMapSate.CurRoomID = mapRoomNode.RoomID;
        UIManager.Instance.MapLogicGO.GetComponent<MapLogic>().SetRolePos();
    }
}
