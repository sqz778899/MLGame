using UnityEngine;

public static class LevelManager
{
    public static LevelMono LoadLevel(int levelID)
    {
        GameObject levelRoot = BattleManager.Instance._MapManager.MapFightRoot.transform.GetChild(0).gameObject;
        //清理之前的Level
        for (int i = levelRoot.transform.childCount - 1; i >=0; i--)
            GameObject.Destroy(levelRoot.transform.GetChild(i).gameObject);
        //Load Map
        GameObject levelIns = ResManager.instance.CreatInstance(PathConfig.GetLevelPath(levelID));
        levelIns.transform.SetParent(levelRoot.transform,false);
        LevelMono curlevel = levelIns.GetComponent<LevelMono>();
        //设置角色位置
        GameObject roleInner = PlayerManager.Instance.RoleInFightGO;
        Vector3 curRolePos = roleInner.transform.position;
        roleInner.transform.position = new Vector3(0, curRolePos.y, 0);
        //Set Award
        return curlevel;
    }
}
