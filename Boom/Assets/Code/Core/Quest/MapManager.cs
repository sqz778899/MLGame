using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform mapRoot;  // 用于挂载地图Prefab的节点
    GameObject currentMap;
    
    [Header("地图内角色")]
    public GameObject Role;
    [Header("地图节点")]
    MapController _mapController;
    MapRoomNode[] _allMapRooms;
    [Header("对话系统脚本")]
    public Dialogue CurDialogue;
    

    private void Awake()
    {
        MainRoleManager.Instance.CurMapManager = this;
    }

    public void InitializeMap(Quest quest)
    {
        LoadMap(quest.ID);
        SetMapDifficulty(quest.DifficultyLevel);
        
    }
    // 加载地图
    void LoadMap(int questID)
    {
        ClearCurrentMap();
        currentMap = ResManager.instance.CreatInstance(PathConfig.MapPB(questID));
        currentMap.transform.SetParent(mapRoot.transform,false);
        _mapController = currentMap.GetComponent<MapController>();
        Role = _mapController.Role;
        MainRoleManager.Instance.MainRoleIns = Role;
        
        //获取地图房间节点
        _allMapRooms = mapRoot.GetComponentsInChildren<MapRoomNode>();
        _allMapRooms.ForEach(e=>e.InitData());
        //设置角色位置
        SetRolePos();
    }

    #region 初始化地图数据
    // 设置地图难度（根据需求自定义实现）
    void SetMapDifficulty(int difficultyLevel)
    {
        if (currentMap == null)
        {
            Debug.LogWarning("当前没有加载地图，无法设置难度");
            return;
        }

        // 假设你的地图Prefab含有MapController脚本
        var controller = currentMap.GetComponent<MapController>();
        if (controller != null)
            controller.SetupDifficulty(difficultyLevel);
        else
            Debug.LogWarning("地图Prefab缺少MapController组件");
    }
    #endregion

    #region 不太关心的各种方法
    // 清除当前地图
    void ClearCurrentMap()
    {
        int childCount = mapRoot.transform.childCount;
        for (int i = childCount -1; i >= 0; i--)
            Destroy(mapRoot.transform.GetChild(i).gameObject);
    }
    
    public void SetAllIDs()
    {
        for (int i = 0; i < _allMapRooms.Length; i++)
            _allMapRooms[i].RoomID = i + 1;
    }
    
    public void SetRolePos()
    {
        //找到当前房间的节点
        MapRoomNode curRoom = _allMapRooms.FirstOrDefault(
            each => each.RoomID == MainRoleManager.Instance.CurMapSate.CurRoomID);
        curRoom.State = MapRoomState.Unlocked;
        
        //设置角色&&摄像机位置
        Role.GetComponent<RoleInMap>().CurRoom = curRoom;
        //Role.GetComponent<RoleInMap>().InitRoomBounds();
        Role.transform.position = curRoom.RoleStartPos.position;
        Vector3 newCameraPos = new Vector3(curRoom.CameraStartPos.position.x,
            curRoom.CameraStartPos.position.y, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f);
    }
    #endregion
}