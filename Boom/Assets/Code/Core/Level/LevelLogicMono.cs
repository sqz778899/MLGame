using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelLogicMono : MonoBehaviour
{
    #region 关卡相关
    public GameObject GroupLevel;

    void InitLevel()
    {
        int curLevelID = MSceneManager.Instance.CurMapSate.LevelID;
        string curLevelName = string.Format("P_Level_{0}.prefab", curLevelID.ToString("D2"));
        Instantiate(ResManager.instance.GetAssetCache<GameObject>
            (PathConfig.LevelAssetDir+curLevelName), GroupLevel.transform);
    }

    public void LoadSceneWin(int SceneID)
    {
        MSceneManager.Instance.WinThisLevel();
        MainRoleManager.Instance.WinOrFailState = WinOrFail.InLevel;
        MSceneManager.Instance.LoadScene(SceneID);
    }
    #endregion

    #region 计分板相关
    public TextMeshProUGUI txtScore;
    #endregion
    
    public bool isBeginCameraMove;
    GameObject FirstBullet;
    public bool isBeginCalculation;
    public GameObject WinGUI;
    public GameObject FailGUI;

    public float Distance;
    public Enemy CurEnemy;
    void Start()
    {
        //InitLevel
        InitLevel();
        isBeginCameraMove = false;
        isBeginCalculation = false;
        UIManager.Instance.InitCharacterLevel();
        Distance = 0f;
        CurEnemy = UIManager.Instance.EnemyILIns.GetComponent<Enemy>();
        MainRoleManager.Instance.WinOrFailState = WinOrFail.InLevel;
    }
    
    void Update()
    {
        txtScore.text = "Score: " + MainRoleManager.Instance.Score;
        if (isBeginCalculation)
            WinOrFailThisLevel();

        if (UIManager.Instance.EnemyILIns != null)
            Distance = Vector2.Distance(UIManager.Instance.EnemyILIns.transform.position,
                UIManager.Instance.CharILIns.transform.position);

        if (isBeginCameraMove && FirstBullet != null)
        {
            Vector3 s = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(FirstBullet.transform.position.x,s.y,s.z);
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
            GameObject curBullet = BulletManager.Instance.InstanceBullet(curB.bulletID,BulletInsMode.Inner,pos);
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
