using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static LevelMono LoadLevel()
    {
        Debug.Log("LoadLevel");
        //清理之前的Level
        for (int i = UIManager.Instance.Level.transform.childCount - 1; i >=0; i--)
        {
            GameObject.DestroyImmediate(UIManager.Instance.Level.transform.GetChild(i).gameObject);
        }
        //Load Map
        GameObject levelIns = ResManager.instance.CreatInstance(
            PathConfig.GetLevelPath(MainRoleManager.Instance.CurMapSate.CurLevelID));
        levelIns.transform.SetParent(UIManager.Instance.Level.transform,false);
        LevelMono curlevel = levelIns.GetComponent<LevelMono>();
        curlevel.SetEnemy(MainRoleManager.Instance.CurEnemyMidData);//初始化敌人属性
        //Set Role
        //设置角色位置
        Vector3 curRolePos = UIManager.Instance.RoleIns.transform.position;
        UIManager.Instance.RoleIns.transform.position = new Vector3(0, curRolePos.y, 0);
        //Set Award
        return curlevel;
    }
}
