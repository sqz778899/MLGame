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

    public bool isBeginCameraMove;
    GameObject FirstBullet;
    public bool isBeginCalculation;
    public GameObject WinGUI;
    public GameObject FailGUI;

    public float Distance;
    public Enemy CurEnemy;
    void Update()
    {
        //开始结算关卡
        if (isBeginCalculation)
            WinOrFailThisLevel();
        
        //实时计算与敌人的距离
        if (CurEnemy != null)
            Distance = Vector2.Distance(CurEnemy.transform.position,
                UIManager.Instance.RoleIns.transform.position);
        
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
        MainRoleManager.Instance.WinOrFailState = WinOrFail.InLevel;
        CreateBulletInner();
    }

    //在开始战斗的时候，根据角色槽位的子弹，创建五个跟着他跑的傻逼嘻嘻的小子弹
    void CreateBulletInner()
    {
        Vector3 startPos = new Vector3(-1.5f, -0.64f, 1f);
        for (int i = 0; i < MainRoleManager.Instance.CurBullets.Count; i++)
        {
            BulletReady curB = MainRoleManager.Instance.CurBullets[i];
            GameObject bulletIns = BulletManager.Instance.
                InstanceBullet(curB.bulletID, BulletInsMode.Inner);
            BulletInner curSC = bulletIns.GetComponent<BulletInner>();
            float offsetX = startPos.x -i * 1.5f;
            curSC.FollowDis = Mathf.Abs(offsetX);
            bulletIns.transform.position = new Vector3(offsetX,startPos.y,startPos.z + i);
        }
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

    #region 开火相关
    public float delay = 0.3f;
    
    public void CheckForKeyPress(Vector3 pos)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Fire(pos);
    }
    public void Fire(Vector3 pos)
    {
        UIManager.Instance.RoleIns.GetComponent<RoleMove>().Fire();
        StartCoroutine(FireWithDelay(pos, delay));
    }
    
    public IEnumerator FireWithDelay(Vector3 pos, float delay)
    {
        Debug.Log("fire");
        List<BulletReady> bulletDatas = MainRoleManager.Instance.CurBullets;
        
        //
        for (int i = 0; i < bulletDatas.Count; i++)
        {
            BulletReady curB = bulletDatas[i];
            GameObject curBullet = BulletManager.Instance.
                InstanceBullet(curB.bulletID,BulletInsMode.Inner,pos);
            if (i == 0)
                FirstBullet = curBullet;
            
            if (curBullet != null)
                curBullet.transform.SetParent(UIManager.Instance.G_BulletInScene.transform);
            yield return new WaitForSeconds(delay);  // 在发射下一个子弹之前，等待delay秒
        }
        
        //.......Camera Move.............
        isBeginCameraMove = true;
        //........Enemy Die..............
        isBeginCalculation = true;     //发射子弹之后，关卡开启结算模式
    }
    #endregion
}
