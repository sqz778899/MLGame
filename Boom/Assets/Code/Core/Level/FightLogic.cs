using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    Transform _firstBulletTrans => 
        CurRole.Bullets[0] != null && CurRole.Bullets[0].gameObject != null 
            ? CurRole.Bullets[0].transform : null;
    [Header("窗口")]
    public BagRootMini BagRootMiniSC;
    public GameObject WarReportRootGUI;
    public GameObject WinGUI;
    public GameObject FailGUI;
    public float Distance;

    [Header("角色")] 
    public Enemy CurEnemy;
    public RoleInner CurRole;
    public bool _isAttacked;

    bool isEnd = false;
    float _cameraCurSpeed = 0f; 
    int _curBulletCount => CurRole.Bullets.Count;

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
        BagRootMiniSC.InitData();
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
        MainRoleManager.Instance.CurWarReport.CurWarIndex += 1;
        MainRoleManager.Instance.MainRoleIns = CurRole.gameObject;
        isCameraStopping = false;
        isBeginCatch = false;
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
    bool isCameraStopping = false;
    bool isBeginCatch = false;
    void HandleCameraFollow()
    {
        if (!isBeginCameraMove) return;

        if (_firstBulletTrans == null)
        {
            if (!isCameraStopping)
            {
                isCameraStopping = true;
                Vector3 startPos = Camera.main.transform.position;
                Vector3 endPos = new Vector3(startPos.x + 2f, startPos.y, startPos.z);
                // 创建一个序列
                Sequence cameraSequence = DOTween.Sequence();
                // 先播放震动效果
                Vector3 sbu = (endPos - startPos) / (_curBulletCount + 1);
                // 连续震动，每次间隔0.5s
                for (int i = 0; i < _curBulletCount; i++)
                {
                    cameraSequence.Append(Camera.main.transform.DOShakePosition(0.3f, strength: new Vector3(0.8f, 0.3f, 0f), vibrato: 20, randomness: 20));
                    cameraSequence.Append(Camera.main.transform.DOMove(startPos + sbu *(i + 1), 0.5f).SetEase(Ease.OutQuad)); // 平滑移动
                }
                // 然后播放平滑移动效果
                cameraSequence.Append(Camera.main.transform.DOMove(endPos, 3f));
                // 可以选择性地在最后添加回调或其它操作
                cameraSequence.OnKill(() =>
                {
                    isCameraStopping = false;
                    isBeginCameraMove = false;
                });
            }
            return;     
        }
    
        Vector3 bulletViewportPos = Camera.main.WorldToViewportPoint(_firstBulletTrans.position);
    
        // 如果子弹快飞出屏幕，镜头开始追
        if (bulletViewportPos.x > 0.9f || bulletViewportPos.x < 0.1f)
            isBeginCatch = true;

        if (isBeginCatch)
        {
            BulletInner firstBullet = CurRole.Bullets[0];
            float curSpeed = firstBullet.CurSpeed; // 直接拿实时速度
            // 镜头的加速度
            _cameraCurSpeed = Mathf.Lerp(_cameraCurSpeed, Mathf.Max(curSpeed * 1.5f, curSpeed + 10f), Time.deltaTime * 14f);
            
            // 目标视口位置永远在屏幕中心 0.5f
            Vector3 targetViewPos = new Vector3(0.5f, 0.5f, 0);
            Vector3 targetWorldPos = Camera.main.ViewportToWorldPoint(targetViewPos);
            Vector3 targetPos = Camera.main.transform.position;
            
            targetPos.x = Mathf.MoveTowards(
                Camera.main.transform.position.x,
                _firstBulletTrans.position.x - (targetWorldPos.x - Camera.main.transform.position.x),
                _cameraCurSpeed * Time.deltaTime);

            Camera.main.transform.position = targetPos;
        }
    }
    
    void HandleInput()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        if (Input.GetKeyDown(KeyCode.Space) && !_isAttacked)
        {
            _isAttacked = true;
            CurRole.Fire();
            isBeginCameraMove = true;
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
        MainRoleManager.Instance.CurWarReport.IsWin = true;
        WarReportRootGUI.SetActive(true);
        UIManager.Instance.WarReportGO.GetComponent<GUIWarReport>().SyncReport();
    }
    
    void FailTheLevel()
    {
        MainRoleManager.Instance.CurWarReport.IsWin = false;
        WarReportRootGUI.SetActive(true);
        UIManager.Instance.WarReportGO.GetComponent<GUIWarReport>().SyncReport();
    }
    
    public void WarReportContinue()
    {
        WarReportRootGUI.SetActive(false);
        if (MainRoleManager.Instance.CurWarReport.IsWin)
        {
            WinGUI.SetActive(true);
            WinGUI.GetComponent<GUIWin>().Win(CurEnemy.CurAward);
        }
        else
        {
            MainRoleManager.Instance.HP -= 1;
            FailGUI.SetActive(true);
            FailGUI.GetComponent<GUIFail>().SetHertAni();
        }
    }
    #endregion
}
