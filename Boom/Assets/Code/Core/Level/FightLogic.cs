using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FightLogic : MonoBehaviour
{
    #region 关卡相关
    public GameObject GroupLevel;
    public GameObject CurLevel;

    void InitLevel()
    {
        int curLevelID = MSceneManager.Instance.CurMapSate.LevelID;
        string curLevelName = string.Format("P_Level_{0}.prefab", curLevelID.ToString("D2"));
        CurLevel = Instantiate(ResManager.instance.GetAssetCache<GameObject>
            (PathConfig.LevelAssetDir+curLevelName), GroupLevel.transform);
    }
    #endregion

    [Header("Display")] 
    public float waitCalculateTime = 3f;
    public bool isBeginCameraMove;
    GameObject FirstBullet;
    public bool isBeginCalculation;
    public GameObject WinGUI;
    public GameObject FailGUI;

    public float Distance;
    public Enemy CurEnemy;
    public RoleInner CurRole;
    void Update()
    {
        //开始结算关卡
        if (isBeginCalculation)
            WinOrFailThisLevel();
        
        //实时计算与敌人的距离
        if (CurEnemy != null)
            Distance = Vector2.Distance(CurEnemy.transform.position,
                UIManager.Instance.RoleIns.transform.position);
        
        //开火
        CheckForKeyPress();
        
        //摄像机跟随子弹命中敌人动画
        if (isBeginCameraMove && FirstBullet != null)
        {
            Vector3 s = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(FirstBullet.transform.position.x,s.y,s.z);
        }
    }
    
    public void InitData()
    {
        //InitLevel
        InitLevel();
        FailGUI.SetActive(false);
        WinGUI.SetActive(false);
        isBeginCameraMove = false;
        isBeginCalculation = false;
        Distance = 0f;
        CurEnemy = CurLevel.GetComponent<LevelMono>().GetCurEnemy();
        CurRole = UIManager.Instance.RoleIns.GetComponent<RoleInner>();
        CurRole.InitData();
        MainRoleManager.Instance.WinOrFailState = WinOrFail.InLevel;
    }

    public void UnloadData()
    {
        //清除场景内遗留子弹
        GameObject root = UIManager.Instance.G_BulletInScene;
        for (int i = root.transform.childCount - 1; i >= 0; i--)
        {
            BulletInner curSC = root.transform.GetChild(i).GetComponent<BulletInner>();
            curSC.DestroySelf();
        }
    }

    void WinOrFailThisLevel()
    {
        //如果子弹为0，且敌人未死则失败
        if (CurEnemy.EState == EnemyState.dead)
            MainRoleManager.Instance.WinOrFailState = WinOrFail.Win;

        if (UIManager.Instance.G_BulletInScene.transform.childCount == 0 &&
            CurEnemy.EState == EnemyState.live)
            MainRoleManager.Instance.WinOrFailState = WinOrFail.Fail;
        
        switch (MainRoleManager.Instance.WinOrFailState)
        {
            case WinOrFail.InLevel:
                break;
            case WinOrFail.Win:
                WinTheLevel();
                break;
            case WinOrFail.Fail:
                FailGUI.SetActive(true);
                isBeginCalculation = false;
                break;
        }
    }
    //
    void WinTheLevel()
    {
        //播放胜利
        WinGUI.SetActive(true);
        isBeginCalculation = false;
        MSceneManager.Instance.WinThisLevel();
        //给一个随机Buff
        //RollManager.Instance.OnceRollBuff();
        //选完了给一个随机宝物
    }

    #region 开火相关;
    public void CheckForKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            FireInvoke();
    }
    public void FireInvoke()
    {
        CurRole.Fire();
    }
    #endregion
}