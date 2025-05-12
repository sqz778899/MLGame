using System;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public MapSate CurMapSate;
    [Header("地图节点")]
    public Transform MapResRoot;  // 用于挂载地图Prefab的节点
    public GameObject MapFightRoot;  // 地图战斗相关加载的节点
    public GameObject MapBuleltRoot;  // 场景内的子弹的父节点
    public MapMouseControl MapMouseControl;  // 地图鼠标控制脚本
    public GameObject PreERIconGO;            //元素反应给外部用的资产
    
    GameObject currentMap;
    MapController _mapController;
    MapRoomNode[] _allMapRooms;
    
    //[Header("一些脚本")] 
    BattleData _battleData => BattleManager.Instance.battleData;
    BattleLogic _battleLogicSC => BattleManager.Instance.battleLogic;  
    
    //[Header("一些UI")] 
    GameObject _GUIUIFightMapRootGO => EternalCavans.Instance.GUIFightMapRootGO;
    GameObject _GUIMapRootGO => EternalCavans.Instance.GUIMapRootGO;
    GameObject _sideBar => EternalCavans.Instance.G_SideBar;
    
    Vector3 _preCameraPos;

    void Awake()
    {
       
        GM.Root.BattleMgr._MapManager = this;
        RoleInner curRoleInFight = MapFightRoot.GetComponentInChildren<RoleInner>(true);
        GM.Root.PlayerMgr.RoleInFightSC = curRoleInFight;
        GM.Root.PlayerMgr.RoleInFightGO = curRoleInFight.gameObject;
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
        GM.Root.PlayerMgr.RoleInMapGO = _mapController.Role;
        GM.Root.PlayerMgr.RoleInMapSC = _mapController.Role.GetComponent<RoleInMap>();
        
        //获取地图房间节点
        _allMapRooms = currentMap.GetComponentsInChildren<MapRoomNode>();
        CurMapSate = new MapSate();
        CurMapSate.AllRoomCount = _allMapRooms.Length;
        _allMapRooms.ForEach(e=>e.InitData());
        //设置角色位置
        ToTargetRoom();
    }
    #endregion
    
    #region 切换
    public void SwitchMapScene()
    {
        if (UIManager.Instance.IsLockedClick) return;
        GetComponent<MapMouseControl>().UnLockMap();
        Camera.main.transform.position = _preCameraPos;
        EternalCavans.Instance.BagButtonGO.SetActive(true);
        FightSceneOff();
        MapSceneOn();
    }
    
    public void SwitchFightScene()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        _preCameraPos = Camera.main.transform.position;
        EternalCavans.Instance.BagButtonGO.SetActive(false);
        GM.Root.PlayerMgr.RoleInFightSC.ClearConnon();
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
        EternalCavans.Instance.ShowMiniBag();
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
        EternalCavans.Instance.HideMiniBag();
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
    public MapRoomNode GetCurRoomNode() => _allMapRooms.FirstOrDefault(r => r.RoomID == CurMapSate.CurRoomID);

    #endregion

    #region 不太关心的各种方法
    public void SetAllRoomIDs() => _allMapRooms.Select((room, index) => room.RoomID = index + 1).ToList();
    
    public void ToCurRoom()
    {
        //找到当前房间的节点
        MapRoomNode CurRoom = _allMapRooms.FirstOrDefault(
            each => each.RoomID == CurMapSate.CurRoomID);
        CurRoom.State = MapRoomState.Unlocked;
        
        //设置角色&&摄像机位置
        SetRoomInfo(CurRoom);
    }
    
    public void ToTargetRoom()
    {
        //找到当前房间的节点
        MapRoomNode targetRoom = _allMapRooms.FirstOrDefault(
            each => each.RoomID == CurMapSate.TargetRoomID);
        
        //设置角色&&摄像机位置
        SetRoomInfo(targetRoom, () =>
        {
            #region 触发 OnEnterRoom 相关道具&&Buff
            bool isFirstEnter = !CurMapSate.IsFinishedRooms.Contains(targetRoom.RoomID);
            // --- 创建Context传参 ---
            BattleContext ctx = new BattleContext
            {
                EnterRoomID = targetRoom.RoomID,
                IsFirstEnterRoom = isFirstEnter
            };
            // 交给道具管理器处理
            GM.Root.InventoryMgr.MiracleOddityMrg.Trigger(MiracleOddityTriggerTiming.OnEnterRoom, ctx);
            #endregion
            // --- 处理房间状态 ---
            CurMapSate.FinishAndToNextRoom();//记录下探索度,并同步当前房间ID
            targetRoom.State = MapRoomState.Unlocked;
        });
    }

    void SetRoomInfo(MapRoomNode roomNode,Action onFinish = null)
    {
        //设置角色&&摄像机位置
        GameObject roleInMap = GM.Root.PlayerMgr.RoleInMapGO;
        roleInMap.GetComponent<RoleInMap>().CurRoom = roomNode;
        roleInMap.transform.position = roomNode.RoleStartPos.position;
        Vector3 newCameraPos = new Vector3(roomNode.CameraStartPos.position.x,
            roomNode.CameraStartPos.position.y, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f).OnComplete(() =>
        {
            roomNode.SetRenderLayer(roleInMap);
            onFinish?.Invoke(); // ✅ 回调
        });
    }
    
    public void UnloadLevelData()
    {
        //清除场景内遗留子弹
        for (int i = MapBuleltRoot.transform.childCount - 1; i >= 0; i--)
            Destroy(MapBuleltRoot.transform.GetChild(i).gameObject);
        //卸载战斗场景
        if (_battleData.CurLevel != null)
            Destroy(_battleData.CurLevel.gameObject);
    }
    
    void OnDestroy()
    {
        EternalCavans.Instance.OnOpenBag -= LockAllThings;
        EternalCavans.Instance.OnCloseBag -= UnLockAllThings;
    }
    #endregion
}