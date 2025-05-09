using System;
using System.Linq;
using UnityEngine;

public class BattleManager: MonoBehaviour
{
    public MapManager _MapManager;
    
    public BattleData battleData;
    public BattleLogic battleLogic;
    public ElementZoneManager elementZoneMgr;
    public BattleUIController battleUI;
    
    //战斗流程控制锁，做时停用
    public BattleFlowLock battleFlowLock { get; private set; } = new();

    //进入战斗唯一入口
    public void EnterFight(EnemyConfigData _enemyConfig,int _levelID)
    {
        InitData();
        //1)进入战斗场景
        _MapManager.SwitchFightScene();
        //2)初始化战斗数据
        battleLogic._battleCameraController = new BattleCameraController();//加载摄像机控制器
        battleData.InitFightData(_enemyConfig, _levelID);
        battleLogic.InitFightData();
        battleUI.InitFightData();
        //3)锁一下地图缩放
        _MapManager.GetComponent<MapMouseControl>().LockMap();
    }
    

    #region 战后UI以及行为
    //胜利结算战报界面
    public void ShowWarReport(bool isWin)
    {
        battleData.CurWarReport.IsWin = isWin;
        battleUI.ShowWarReport();
    }
    
    public void WarReportContinue() => battleUI.WarReportContinue();
    
    //赢得战斗
    public void WinToNextRoom()
    {
        battleUI.InitWinFailGUI();
        //结算之后抽一发奖励
        ShowWinReward(() =>
        {
            _MapManager.SwitchMapScene();
            _MapManager.ToTargetRoom();
            GM.Root.PlayerMgr._PlayerData.OnBattleEnd(); // Buff结算
        });
    }
    public void ShowWinReward(Action onFinish)
    {
        // 创建奖励面板
        RollAward rollAwardUI = EternalCavans.Instance.RollAwardSC;
        rollAwardUI.ShowReward(onFinish);
    }
    
    //战斗失败
    public void FailToThisRoom()
    {
        //Debug.Log("FailToThisRoom");
        battleUI.InitWinFailGUI();
        _MapManager.SwitchMapScene();
        _MapManager.ToCurRoom();
        GM.Root.PlayerMgr._PlayerData.OnBattleEnd();//Buff结算
    }
    #endregion
    
    #region 单例的加载卸载
    public static BattleManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        battleData = ResManager.instance.GetAssetCache<BattleData>(PathConfig.BattleDataPath);
        elementZoneMgr = new ElementZoneManager();
        elementZoneMgr.InitData();
    }
    #endregion
    
    #region 初始化以及事件的注册和注销
    void InitData()
    {
        battleLogic ??= GameObject.Find("BattleLogic").GetComponent<BattleLogic>();
        battleUI ??= new BattleUIController();
    }
    void Start()
    {
        EternalCavans.Instance.OnFightContinue += WarReportContinue;
        EternalCavans.Instance.OnWinToNextRoom += WinToNextRoom;
        EternalCavans.Instance.OnFailToThisRoom += FailToThisRoom;
    }
    void OnDestroy()
    {
        EternalCavans.Instance.OnFightContinue -= WarReportContinue;
        EternalCavans.Instance.OnWinToNextRoom -= WinToNextRoom;
        EternalCavans.Instance.OnFailToThisRoom -= FailToThisRoom;
    }
    #endregion
}

public class BattleFlowLock
{
    public bool IsReactionPlaying { get; private set; }

    public void LockReaction() => IsReactionPlaying = true;
    public void UnlockReaction() => IsReactionPlaying = false;
}
