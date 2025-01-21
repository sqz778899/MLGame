using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static LevelMono LoadLevel(int levelID)
    {
        Debug.Log("LoadLevel");
        //Load Map
        GameObject levelIns = ResManager.instance.CreatInstance(PathConfig.GetLevelPath(levelID));
        levelIns.transform.SetParent(UIManager.Instance.Level.transform,false);
        LevelMono curlevel = levelIns.GetComponent<LevelMono>();
        //Sync Enemy
        //Set Role
        //设置角色位置
        Vector3 curRolePos = UIManager.Instance.RoleIns.transform.position;
        UIManager.Instance.RoleIns.transform.position = new Vector3(0, curRolePos.y, 0);
        //Set Award
        return curlevel;
    }
}
