using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MapLogic : MonoBehaviour
{
    public GameObject MapNodeRoot;
    [Header("地图内角色")]
    public GameObject Role;
    [Header("对话系统脚本")]
    public Dialogue CurDialogue;
    [Header("地图节点")]
    public List<GameObject> MapRooms;
    MapRoomNode[] _allMapRooms;
    
    void Start()
    {
        MainRoleManager.Instance.MainRoleIns = Role;
        InitMapData();
        
        Dialogue curDia = CurDialogue;
        curDia.LoadDialogue("Beginner01");
    }

    public void InitMapData()
    {
        //获取地图房间节点
        MapRooms = new List<GameObject>();
        _allMapRooms = MapNodeRoot.GetComponentsInChildren<MapRoomNode>();
        foreach (var each in _allMapRooms)
            MapRooms.Add(each.gameObject);
        
        //设置角色位置
        SetRolePos();
    }
    
    public void SetRolePos()
    {
        //找到当前房间的节点
        MapRoomNode curRoom = _allMapRooms.FirstOrDefault(
            each => each.RoomID == MainRoleManager.Instance.CurMapSate.CurRoomID);

        //设置角色&&摄像机位置
        Role.GetComponent<RoleInMap>().CurRoom = curRoom;
        Role.transform.position = curRoom.RoleStartPos.position;
        Vector3 newCameraPos = new Vector3(curRoom.CameraStartPos.position.x,
            curRoom.CameraStartPos.position.y, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f);
    }

    public void SetAllIDs()
    {
        for (int i = 0; i < _allMapRooms.Length; i++)
            _allMapRooms[i].RoomID = i + 1;
    }
}
