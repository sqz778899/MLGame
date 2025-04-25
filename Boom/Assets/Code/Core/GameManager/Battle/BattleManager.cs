using System.Collections.Generic;
using UnityEngine;

public class BattleManager: MonoBehaviour
{
    public MapManager _MapManager;
    
    public BattleData battleData;
    public BattleLogic battleLogic;
    public BattleUIController battleUI;

    public bool IsInBattle = false;

    #region 事件的注册和注销
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

    //进入战斗唯一入口
    public void EnterFight(EnemyConfigData _enemyConfig,int _levelID)
    {
        InitData();
        IsInBattle = true;
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
    
    public void WarReportContinue()
    {
        battleUI.WarReportContinue();
    }
    
    //赢得战斗
    public void WinToNextRoom()
    {
        IsInBattle = false;
        battleUI.InitWinFailGUI();
        _MapManager.SwitchMapScene();
        _MapManager.ToTargetRoom();
        GM.Root.PlayerMgr._PlayerData.BuffManager.OnBattleEnd(); //Buff结算
    }

    //战斗失败
    public void FailToThisRoom()
    {
        IsInBattle = false;
        battleUI.InitWinFailGUI();
        _MapManager.SwitchMapScene();
        _MapManager.ToCurRoom();
        GM.Root.PlayerMgr._PlayerData.BuffManager.OnBattleEnd();//Buff结算
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
    }
    #endregion
    
    void InitData()
    {
        battleLogic ??= GameObject.Find("BattleLogic").GetComponent<BattleLogic>();
        battleUI ??= new BattleUIController();
    }
}