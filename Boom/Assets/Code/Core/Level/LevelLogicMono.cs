using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLogicMono : MonoBehaviour
{
    #region 关卡相关
    public GameObject GroupLevel;
    void Start()
    {
        //InitLevel
        int curLevelID = MSceneManager.Instance.LevelID;
        string curLevelName = string.Format("P_Level_{0}.prefab", curLevelID.ToString("D2"));
        Instantiate(ResManager.instance.GetAssetCache<GameObject>
               (PathConfig.LevelAssetDir+curLevelName), GroupLevel.transform);
    }
    #endregion

    #region 计分板相关
    public TextMeshProUGUI txtScore;
    #endregion

    public GameObject WinGUI;
    public GameObject FailGUI;
    void Update()
    {
        txtScore.text = "Score: " + CharacterManager.Instance.Score;
        WinOrFailThisLevel();
    }

    void WinOrFailThisLevel()
    {
        switch (CharacterManager.Instance.WinOrFailState)
        {
            case WinOrFail.InLevel:
                break;
            case WinOrFail.Win:
                WinGUI.SetActive(true);
                break;
            case WinOrFail.Fail:
                FailGUI.SetActive(true);
                break;
        }
    }

    #region 开火相关
    public float delay = 0.3f;
    List<BulletDataJson> BulletDesignJsons;
    GameObject GroupBullet;
    
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
        yield return new WaitForSeconds(delay);  // 等待delay秒，然后开始发射子弹

        Debug.Log("fire");
    
        List<BulletData> bulletDatas = CharacterManager.Instance.Bullets;
        BulletDesignJsons = CharacterManager.Instance.BulletDesignJsons;
    
        if (GroupBullet == null)
            GroupBullet = GameObject.Find("GroupBullet");
    
        foreach (BulletData eBuDT in bulletDatas)
        {
            GameObject curBullet = eBuDT.InstanceBullet(BulletDesignJsons,pos);
            if (curBullet != null && GroupBullet != null)
                curBullet.transform.SetParent(GroupBullet.transform);
        
            yield return new WaitForSeconds(delay);  // 在发射下一个子弹之前，等待delay秒
        }
    }
    #endregion
}
