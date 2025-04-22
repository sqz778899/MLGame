#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class MEditTool
{
    public static bool BindArrowTargetRoom(MapNodeDataConfigMono arrowNode, float stepDistance = 0.5f, int maxSteps = 20, float checkRadius = 0.2f)
    {
        if (arrowNode == null || arrowNode.RoomArrowConfig == null)
        {
            Debug.LogWarning("[ArrowBindUtility] 传入无效箭头");
            return false;
        }

        if (arrowNode._MapEventType != MapEventType.RoomArrow)
            return false;

        GameObject arrowGO = arrowNode.gameObject;
        Vector3 startPos = arrowGO.transform.position;
        MapRoomNode selfRoom = arrowGO.GetComponentInParent<MapRoomNode>();

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

        MapRoomNode firstHitRoom = null;
        float shortestDist = float.MaxValue;
        Vector3 resultDir = Vector3.zero;
        
        MapRoomNode[] allRooms = GameObject.FindObjectsOfType<MapRoomNode>();
        
        foreach (Vector3 dir in directions)
        {
            for (int i = 1; i <= maxSteps; i++)
            {
                Vector3 checkPos = startPos + dir * stepDistance * i;

                foreach (var room in allRooms)
                {
                    if (room == selfRoom) continue;
                    SpriteRenderer renderer = room.GetComponentInChildren<SpriteRenderer>();
                    if (renderer != null && renderer.bounds.Contains(checkPos))
                    {
                        float dist = Vector3.Distance(startPos, checkPos);
                        if (dist < shortestDist)
                        {
                            firstHitRoom = room;
                            shortestDist = dist;
                            resultDir = dir;
                        }
                        break;
                    }
                }
            }
        }
        
        if (firstHitRoom != null)
        {
            Undo.RecordObject(arrowNode, "自动绑定箭头房间");
            
            if (resultDir == Vector3.right)
                arrowNode.transform.eulerAngles = new Vector3(0, 0, 270);
            if (resultDir == Vector3.left)
                arrowNode.transform.eulerAngles = new Vector3(0, 0, 90);
            if (resultDir == Vector3.up)
                arrowNode.transform.eulerAngles = new Vector3(0, 0, 0);
            if (resultDir == Vector3.down)
                arrowNode.transform.eulerAngles = new Vector3(0, 0,180);
            
            arrowNode.RoomArrowConfig.TargetRoomID = firstHitRoom.RoomID;
            //Debug.Log($"✅ 成功绑定：{arrowGO.name} → 房间ID {firstHitRoom.RoomID}");
            return true;
        }
        Debug.LogWarning($"❌ {arrowGO.name} 没有命中任何房间");
        return false;
    }
}
#endif