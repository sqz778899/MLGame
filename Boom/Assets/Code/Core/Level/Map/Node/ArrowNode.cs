using System.Collections;
using UnityEngine;

public class ArrowNode : MapNodeBase
{
    [Header("重要属性")]
    public MapRoomNode mapRoomNode;

    internal override void OnMouseEnter()
    {
        spriteRenderer.color = HeighLightColor;// 将精灵高亮显示
        if (Input.GetMouseButtonDown(0))
            transform.localScale = defaultScale * 0.8f;
        if (Input.GetMouseButtonUp(0))
            transform.localScale = defaultScale;
    }

    internal override void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;// 取消高亮显示
    }
    
    public void GoToOtherNode()
    {
        MainRoleManager.Instance.CurMapSate.CurRoomID = mapRoomNode.RoomID;
        UIManager.Instance.MapLogicGO.GetComponent<MapLogic>().SetRolePos();
        TrunkManager.Instance.IsGamePause = true;
        StartCoroutine(CreatDialogueFight());
    }
    
    IEnumerator CreatDialogueFight()
    {
        yield return new WaitForSeconds(0.8f);
        DislogueManager.CreatDialogueFight(mapRoomNode.RoomID);
        TrunkManager.Instance.IsGamePause = false;
    }
    
}
