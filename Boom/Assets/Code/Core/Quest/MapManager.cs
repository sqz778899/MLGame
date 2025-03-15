using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    [Header("地图节点")]
    public Transform MapResRoot;  // 用于挂载地图Prefab的节点
    public GameObject MapFightRoot;  // 地图战斗相关加载的节点
    public GameObject MapBuleltRoot;  // 场景内的子弹的父节点
    public MapMouseControl MapMouseControl;  // 地图鼠标控制脚本
  
    [Header("角色")]
    public GameObject Role;//地图内角色
    public RoleInner RoleInFight;//战斗内角色
   
    GameObject currentMap;
    MapController _mapController;
    MapRoomNode[] _allMapRooms;
    
    [Header("对话系统脚本")]
    public Dialogue CurDialogue;
    
    GameObject _GUIUIFightMapRootGO => UIManager.Instance.MapUI.GUIFightMapRootGO;
    GameObject _GUIMapRootGO => UIManager.Instance.MapUI.GUIMapRootGO;
    BattleLogic _battleLogicSC => UIManager.Instance.Logic.BattleLogicSC;
    GameObject _sideBar => UIManager.Instance.CommonUI.G_SideBar;
    
    Vector3 _preCameraPos;

    public bool IsTest;//是否是测试模式

    void Awake()
    {
        //todo 单独启动场景时候的初始化
        if (IsTest)
        {
            TrunkManager.Instance.ForceRefresh();
            UIManager.Instance.InitStartGame();
            SaveManager.LoadSaveFile();
            MainRoleManager.Instance.InitData();
        }
        //todo ......................
        UIManager.Instance.InitLogic();
        MainRoleManager.Instance.CurMapManager = this;
        EternalCavans.Instance.InMapScene();
        EternalCavans.Instance.OnOpenBag += LockAllThings;
        EternalCavans.Instance.OnCloseBag += UnLockAllThings;
        EternalCavans.Instance.OnWinToNextRoom += WinToNextGame;
        EternalCavans.Instance.OnSwitchMapScene += SwitchMapScene;
    }
    
    void LockAllThings()
    {
        MapMouseControl.LockMap();
        _allMapRooms.ForEach(e=> e.LockRes());
    }
    
    void UnLockAllThings()
    {
        MapMouseControl.UnLockMap();
        _allMapRooms.ForEach(e=> e.UnLockRes());
    }

    #region 加载地图相关
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
        currentMap.transform.SetParent(MapResRoot.transform,false);
        _mapController = currentMap.GetComponent<MapController>();
        Role = _mapController.Role;
        MainRoleManager.Instance.MainRoleIns = Role;
        
        //获取地图房间节点
        _allMapRooms = currentMap.GetComponentsInChildren<MapRoomNode>();
        _allMapRooms.ForEach(e=>e.InitData());
        //设置角色位置
        SetRolePos();
    }
    #endregion
    
    #region 切换
    public void SwitchMapScene()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        Camera.main.transform.position = _preCameraPos;
        UIManager.Instance.BagUI.HideBag();
        FightSceneOff();
        MapSceneOn();
    }
    
    public void SwitchFightScene()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        _preCameraPos = Camera.main.transform.position;
        UIManager.Instance.BagUI.HideBag();
        MapSceneOff();
        FightSceneOn();
    }
    #endregion
    
    #region 切换独立小开关
    void MapSceneOn()
    {
        MapResRoot.gameObject.SetActive(true);
        _GUIMapRootGO.SetActive(true);
        try { enabled = true; }catch (Exception e) {}
    }
    
    void MapSceneOff()
    {
        _GUIMapRootGO.SetActive(false);
        MapResRoot.gameObject.SetActive(false);
        try { enabled = false; }catch (Exception e) {}
    }

    void FightSceneOn()
    {
        _GUIUIFightMapRootGO.SetActive(true);
        UIManager.Instance.BagUI.ShowMiniBag();
        try { _battleLogicSC.enabled = true; }catch (Exception e) {}
        
        for (int i = 0; i < MapFightRoot.transform.childCount; i++)
            MapFightRoot.transform.GetChild(i).gameObject.SetActive(true);
        try
        {
            _battleLogicSC.InitData();
            _sideBar.SetActive(false);
        }
        catch (Exception e) { Debug.LogError("FightSceneOn  Erro！！！"); }
    }
    
    void FightSceneOff()
    {
        _GUIUIFightMapRootGO.SetActive(false);
        UIManager.Instance.BagUI.HideMiniBag();
        try { _battleLogicSC.enabled = false; }catch (Exception e) {}
        for (int i = 0; i < MapFightRoot.transform.childCount; i++)
            MapFightRoot.transform.GetChild(i).gameObject.SetActive(false);
        try
        {
            _battleLogicSC.UnloadData();
            _sideBar.SetActive(true);
        }catch (Exception e) { Debug.LogError("FightSceneOff  Erro！！！"); }
    }
    #endregion

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
        int childCount = MapResRoot.transform.childCount;
        for (int i = childCount -1; i >= 0; i--)
            Destroy(MapResRoot.transform.GetChild(i).gameObject);
    }
    
    public void SetAllIDs()
    {
        for (int i = 0; i < _allMapRooms.Length; i++)
            _allMapRooms[i].RoomID = i + 1;
    }

    public void WinToNextGame()
    { 
        SwitchMapScene();
        MainRoleManager.Instance.WinThisLevel();
        SetRolePos();
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

    void OnDestroy()
    {
        EternalCavans.Instance.OnOpenBag -= LockAllThings;
        EternalCavans.Instance.OnCloseBag -= UnLockAllThings;
        EternalCavans.Instance.OnSwitchMapScene -= SwitchMapScene;
        EternalCavans.Instance.OnWinToNextRoom -= WinToNextGame;
    }
    #endregion
}