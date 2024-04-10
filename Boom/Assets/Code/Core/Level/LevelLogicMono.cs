using System;
using System.Collections;
using System.Collections.Generic;
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
        GameObject LevelIns = Instantiate(ResManager.instance.GetAssetCache<GameObject>
            (PathConfig.LevelAssetDir+curLevelName), GroupLevel.transform);
        Enemy = LevelIns.GetComponentInChildren<Enemy>().gameObject;
    }

    public void LoadSceneWin(int SceneID)
    {
        MSceneManager.Instance.WinThisLevel();
        CharacterManager.Instance.WinOrFailState = WinOrFail.InLevel;
        MSceneManager.Instance.LoadScene(SceneID);
    }
    #endregion

    #region 计分板相关
    public TextMeshProUGUI txtScore;
    #endregion

    public bool isBeginCalculation;
    public GameObject WinGUI;
    public GameObject FailGUI;
    public GameObject Enemy;

    void Start()
    {
        //InitLevel
        InitLevel();
        isBeginCalculation = false;
    }
    void Update()
    {
        txtScore.text = "Score: " + CharacterManager.Instance.Score;
        if (isBeginCalculation)
            WinOrFailThisLevel();
    }

    void WinOrFailThisLevel()
    {
        //如果子弹为0，且敌人未死则失败
        if (UIManager.Instance.GroupBullet.transform.childCount == 0 && Enemy != null)
            if ( Enemy.GetComponent<Enemy>().health > 0)
                CharacterManager.Instance.WinOrFailState = WinOrFail.Fail;
        
        switch (CharacterManager.Instance.WinOrFailState)
        {
            case WinOrFail.InLevel:
                break;
            case WinOrFail.Win:
                WinGUI.SetActive(true);
                isBeginCalculation = false;
                break;
            case WinOrFail.Fail:
                FailGUI.SetActive(true);
                isBeginCalculation = false;
                break;
        }
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
        List<BulletReady> bulletDatas = CharacterManager.Instance.CurBullets;
    
        foreach (BulletReady eBuDT in bulletDatas)
        {
            GameObject curBullet = BulletManager.Instance.InstanceBullet(eBuDT.bulletID
                ,BulletInsMode.Inner,pos);
            if (curBullet != null && UIManager.Instance.GroupBullet != null)
                curBullet.transform.SetParent(UIManager.Instance.GroupBullet.transform);
            yield return new WaitForSeconds(delay);  // 在发射下一个子弹之前，等待delay秒
        }
        isBeginCalculation = true;     //发射子弹之后，关卡开启结算模式
    }
    #endregion
}
