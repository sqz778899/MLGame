using System;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("地图节点")]
    public Transform MapResRoot;  // 用于挂载地图Prefab的节点
    public GameObject MapFightRoot;  // 地图战斗相关加载的节点
    public GameObject MapBuleltRoot;  // 场景内的子弹的父节点
    public MapMouseControl MapMouseControl;  // 地图鼠标控制脚本
    
    GameObject currentMap;
    MapController _mapController;
    MapRoomNode[] _allMapRooms;
    
    [Header("一些脚本")] 
    BattleData _battleData;
    BattleLogic _battleLogicSC;
    
    [Header("一些UI")] 
    GameObject _GUIUIFightMapRootGO;
    GameObject _GUIMapRootGO;
    GameObject _sideBar;
    
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
            UIManager.Instance.BagUI.InitAllBagGO();
        }
        //todo ......................
        UIManager.Instance.InitLogic();
        BattleManager.Instance._MapManager = this;
        PlayerManager.Instance.RoleInFightGO = MapFightRoot.GetComponentInChildren<RoleInner>(true).gameObject;
        EternalCavans.Instance.InMapScene();
        EternalCavans.Instance.OnOpenBag += LockAllThings;
        EternalCavans.Instance.OnCloseBag += UnLockAllThings;
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
        //先清理下老的地图
        MapResRoot.transform.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        currentMap = ResManager.instance.CreatInstance(PathConfig.MapPB(questID));
        currentMap.transform.SetParent(MapResRoot.transform,false);
        _mapController = currentMap.GetComponent<MapController>();
        PlayerManager.Instance.RoleInMapGO = _mapController.Role;
        PlayerManager.Instance.RoleInMapSC = _mapController.Role.GetComponent<RoleInMap>();
        
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
        BattleManager.Instance.battleLogic.enabled = true;
        try { _battleLogicSC.enabled = true; }catch (Exception e) {}
        
        for (int i = 0; i < MapFightRoot.transform.childCount; i++)
            MapFightRoot.transform.GetChild(i).gameObject.SetActive(true);
        try { _sideBar.SetActive(false); }
        catch (Exception e) { Debug.LogError("FightSceneOn  Erro！！！"); }
    }
    
    void FightSceneOff()
    {
        _GUIUIFightMapRootGO.SetActive(false);
        UIManager.Instance.BagUI.HideMiniBag();
        BattleManager.Instance.battleLogic.enabled = false;
        try { _battleLogicSC.enabled = false; }catch (Exception e) {}
        for (int i = 0; i < MapFightRoot.transform.childCount; i++)
            MapFightRoot.transform.GetChild(i).gameObject.SetActive(false);
        try
        {
            UnloadLevelData();
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

    #region 外部调用的各种关于地图信息的接口
    public MapRoomNode GetMapRoomNode(int roomID) => _allMapRooms.FirstOrDefault(r => r.RoomID == roomID);

    #endregion

    #region 不太关心的各种方法
    public void SetAllRoomIDs() => _allMapRooms.Select((room, index) => room.RoomID = index + 1).ToList();
    
    public void SetRolePos()
    {
        InitData();
        //找到当前房间的节点
        MapRoomNode curRoom = _allMapRooms.FirstOrDefault(
            each => each.RoomID == _battleData.CurMapSate.CurRoomID);
        curRoom.State = MapRoomState.Unlocked;
        
        //设置角色&&摄像机位置
        GameObject roleInMap = PlayerManager.Instance.RoleInMapGO;
        roleInMap.GetComponent<RoleInMap>().CurRoom = curRoom;
        roleInMap.transform.position = curRoom.RoleStartPos.position;
        Vector3 newCameraPos = new Vector3(curRoom.CameraStartPos.position.x,
            curRoom.CameraStartPos.position.y, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f);
    }
    
    public void UnloadLevelData()
    {
        InitData();
        //清除场景内遗留子弹
        for (int i = MapBuleltRoot.transform.childCount - 1; i >= 0; i--)
            Destroy(MapBuleltRoot.transform.GetChild(i).gameObject);
        //卸载战斗场景
        if (_battleData.CurLevel != null)
            Destroy(_battleData.CurLevel.gameObject);
    }

    void InitData()
    {
        _battleData ??= BattleManager.Instance.battleData;
        _battleLogicSC ??= BattleManager.Instance.battleLogic;

        _GUIUIFightMapRootGO ??= UIManager.Instance.MapUI.GUIFightMapRootGO;
        _GUIMapRootGO ??= UIManager.Instance.MapUI.GUIMapRootGO;
        _sideBar ??= UIManager.Instance.CommonUI.G_SideBar;
    }

    void OnDestroy()
    {
        EternalCavans.Instance.OnOpenBag -= LockAllThings;
        EternalCavans.Instance.OnCloseBag -= UnLockAllThings;
    }
    #endregion
}