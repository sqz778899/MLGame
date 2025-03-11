using UnityEngine;
using UnityEngine.Serialization;

public class BattleLogic : MonoBehaviour
{
    [Header("关卡&&状态机")] 
    public LevelMono CurLevel;
    IFightState currentState;
    
    [Header("角色")] 
    public Enemy CurEnemy;
    public RoleInner CurRole;
    
    [Header("Display")] 
    public float Distance;            //与敌人的距离
    public bool IsBattleEnded;        //战斗是否结束
    public bool IsAttacking;          //是否正在攻击
    public bool IsAfterAttack;           //是否被已经攻击过了
    public bool IsBeginCameraMove;    //是否开始摄像机移动,外部唯一关心参数
    BattleCameraController _battleCameraController;
   
    [Header("GUI窗口相关")]
    public BagRootMini BagRootMiniSC;
    public EnemyMiniMap EnemyMiniMapSC;
    public GameObject WarReportRootGUI;
    public GameObject WinGUI;
    public GameObject FailGUI;
    BattleUIController _battleUIController;

    void Start()
    {
        TrunkManager.Instance.IsGamePause = false;
    }
    
    void Update()
    {
        if (IsBattleEnded) return;
        
        // 每帧调用当前状态的更新逻辑
        currentState?.Update();
        
        //摄像机跟随子弹命中敌人动画
        _battleCameraController.HandleCameraFollow();
    }

    public void InitData()
    {
        //加载关卡
        LevelManager.InitLevel(this);
        //加载摄像机控制器
        _battleCameraController = new BattleCameraController(this);//加载摄像机控制器
        _battleUIController = new BattleUIController(this);
        CurRole.InitData(CurLevel);//初始化角色数据
        //初始化状态机
        ChangeState(new InLevelState(this));
        //初始化各类数据
        IsBattleEnded = false;
        IsAttacking = false;
        IsAfterAttack = false;
        Distance = 0f;
    }
   
    // 切换状态机
    public void ChangeState(IFightState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool IsBattleOver()
    {
        if (!IsAfterAttack)//如果还没有攻击过,则不会结束
            return false;

        if (CurEnemy.EState == EnemyState.dead)
            return true;
        
        bool isOver = true;
        foreach (var each in CurRole.Bullets)
        {
            if (each != null)
            {
                isOver = false;
                break;
            }
        }
        
        return isOver;
    }
    
    public bool CurrentEnemyIsDead()
    {
        return (CurEnemy != null && CurEnemy.EState == EnemyState.dead);
    }

    //UI调用模块
    public void ShowWinUI() => _battleUIController.ShowWinUI();
    public void ShowFailUI() => _battleUIController.ShowFailUI();
    public void WarReportContinue() => _battleUIController.WarReportContinue();
    
    //卸载战斗场景
    public void UnloadData()
    {
        //清除场景内遗留子弹
        GameObject root = UIManager.Instance.G_BulletInScene;
        for (int i = root.transform.childCount - 1; i >= 0; i--)
            Destroy(root.transform.GetChild(i).gameObject);
        //卸载战斗场景
        if (CurLevel != null)
            Destroy(CurLevel);
    }
}
