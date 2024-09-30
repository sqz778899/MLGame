using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowNode : SpriteClickHandler
{
    [FormerlySerializedAs("MapNode")] [Header("重要属性")]
    public MapRoomNode mapRoomNode;

    public void GoToOtherNode()
    {
        MainRoleManager.Instance.CurLevelID = mapRoomNode.RoomID;
        UIManager.Instance.MapLogicGO.GetComponent<MapLogicNew>().SetRolePos();
    }
}
