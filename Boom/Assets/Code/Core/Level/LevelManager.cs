using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static void LoadLevel(int levelID)
    {
        Debug.Log("LoadLevel");
        //Load Map
        GameObject levelIns = ResManager.instance.CreatInstance(PathConfig.GetLevelPath(levelID));
        //Sync Enemy
        //Set Role
        //Set Award
    }
}
