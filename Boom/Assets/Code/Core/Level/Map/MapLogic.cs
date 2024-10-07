using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MapLogic : MonoBehaviour
{
    public GameObject MapNodeRoot;
    [Header("地图内角色")]
    public GameObject Role;
    [Header("地图节点")]
    public List<GameObject> MapRooms;
    MapRoomNode[] _allMapRooms;
    MapNodeBase[] _allMapNodes;
    
    public GameObject CurMapRoom;
    
    void Start()
    {
        InitMapData();
    }

    public GameObject GetMapNodeByID(int mapNodeID)
    {
        GameObject mapNode = null;
        foreach (var each in _allMapNodes)
        {
            if (each.MapNodeID == mapNodeID)
            {
                mapNode = each.gameObject;
                break;
            }
        }
        return mapNode;
    }

    public void InitMapData()
    {
        //获取地图房间节点
        MapRooms = new List<GameObject>();
        _allMapRooms = MapNodeRoot.GetComponentsInChildren<MapRoomNode>();
        foreach (var each in _allMapRooms)
            MapRooms.Add(each.gameObject);
        _allMapNodes = MapNodeRoot.GetComponentsInChildren<MapNodeBase>();//获取全部地图节点

        //更新地图节点状态
        MapSate CurMapSate = MainRoleManager.Instance.CurMapSate;
        foreach (var eachFinishedNodeID in CurMapSate.IsFinishedMapNodes)
        {
            foreach (var eachMapNode in _allMapNodes)
            {
                if (eachMapNode.MapNodeID == eachFinishedNodeID)
                    eachMapNode.QuitNode();
            }
        }
        
        //设置角色位置
        SetRolePos();
    }
    
    public void SetRolePos()
    {
        //设置角色位置
        foreach (var each in _allMapRooms)
        {
            if (each.RoomID == MainRoleManager.Instance.CurMapSate.CurRoomID)
            {
                Debug.Log($"{each.gameObject.name}  {each.transform.position}");
                Role.transform.position = new Vector3(each.transform.position.x,
                    each.transform.position.y - 3.59f, Role.transform.position.z);
                CurMapRoom = each.gameObject;
                break;
            }
        }
        
        //设置摄像机位置
        Vector3 newCameraPos = new Vector3(Role.transform.position.x, Role.transform.position.y + 3.59f, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f);
    }

    public void SetAllIDs()
    {
        for (int i = 0; i < _allMapRooms.Length; i++)
            _allMapRooms[i].RoomID = i + 1;
        
        for (int i = 0; i < _allMapNodes.Length; i++)
            _allMapNodes[i].MapNodeID = i + 1;
    }
}
