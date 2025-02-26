using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightLogic : MonoBehaviour
{
    Dictionary<WinOrFail, Action> stateActions;
    
    [Header("关卡")] 
    LevelMono CurLevel;
    void InitLevel() => CurLevel = LevelManager.LoadLevel();
    [Header("Display")] 
    public float waitCalculateTime = 3f;
    public bool isBeginCameraMove;
    GameObject FirstBullet;
    public GameObject WinGUI;
    public GameObject FailGUI;
    public GameObject GameOverGUI;
    public float Distance;

    [Header("角色")] 
    public Enemy CurEnemy;
    public RoleInner CurRole;
    public bool _isAttacked;

    bool isEnd = false;

    void Start()
    {
        TrunkManager.Instance.IsGamePause = false;
        stateActions = new Dictionary<WinOrFail, Action>
        {
            { WinOrFail.InLevel, () => {} }, // 空操作
            { WinOrFail.Win, WinTheLevel },
            { WinOrFail.Fail,FailTheLevel }
        };
    }
    
    void Update()
    {
        if (isEnd) return;
        
        //开始结算关卡
        if (CurRole.Bullets.Count == 0 || CurEnemy.EState == EnemyState.dead)
        {
            StartCoroutine( WinOrFailThisLevel());
            isEnd = true;
        }

        //实时计算与敌人的距离
        UpdateDistance();
        //开火
        HandleInput();
        //摄像机跟随子弹命中敌人动画
        HandleCameraFollow();
    }

    public void InitData()
    {
        Camera.main.transform.position = new Vector3(2.5f,1,-10);
        Camera.main.orthographicSize = 5;
        //InitLevel
        InitLevel();
        FailGUI.SetActive(false);
        WinGUI.SetActive(false);
        isEnd = false;
        _isAttacked = false;
        isBeginCameraMove = false;
        Distance = 0f;
        CurEnemy = CurLevel.CurEnemy;
        CurRole = UIManager.Instance.RoleIns.GetComponent<RoleInner>();
        CurRole.InitData(CurLevel);
        MainRoleManager.Instance.WinOrFailState = WinOrFail.InLevel;
    }

    public void UnloadData()
    {
        //清除场景内遗留子弹
        GameObject root = UIManager.Instance.G_BulletInScene;
        for (int i = root.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(root.transform.GetChild(i).gameObject);
        }
        //卸载战斗场景
        if (CurLevel != null)
            DestroyImmediate(CurLevel);
    }

    #region UpDate中的各种状态
    void HandleCameraFollow()
    {
        if (!isBeginCameraMove || FirstBullet == null) return;
    
        Vector3 cameraPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(FirstBullet.transform.position.x, cameraPos.y, cameraPos.z);
    }
    
    void HandleInput()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        if (Input.GetKeyDown(KeyCode.Space) && !_isAttacked)
        {
            _isAttacked = true;
            CurRole.Fire();
        }
    }
    
    void UpdateDistance()
    {
        if(CurEnemy == null) return;
        
        Distance = Vector2.Distance(CurEnemy.transform.position,
            CurRole.transform.position);
    }
    
    //等待结算时间，时间到之后开启结算。。。。。
    IEnumerator WinOrFailThisLevel()
    {
        yield return new WaitForSeconds(waitCalculateTime);
        
        if (CurEnemy.EState == EnemyState.dead)
            MainRoleManager.Instance.WinOrFailState = WinOrFail.Win;
        //如果子弹为0，且敌人未死则失败
        if (CurRole.Bullets.Count == 0 &&
            CurEnemy.EState == EnemyState.live)
            MainRoleManager.Instance.WinOrFailState = WinOrFail.Fail;
        
        stateActions[MainRoleManager.Instance.WinOrFailState]?.Invoke();
    }
    
    //胜利
    void WinTheLevel()
    {
        //播放胜利
        WinGUI.SetActive(true);
        WinGUI.GetComponent<GUIWin>().Win(CurEnemy.CurAward);
    }
    
    void FailTheLevel()
    {
        //播放胜利
        MainRoleManager.Instance.HP -= 1;
        if (MainRoleManager.Instance.HP > 0)
            FailGUI.SetActive(true);
        else
            GameOverGUI.SetActive(true);
    }
    #endregion
}
