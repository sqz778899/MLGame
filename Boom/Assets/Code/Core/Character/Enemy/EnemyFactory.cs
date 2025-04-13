using UnityEngine;

public static class EnemyFactory
{
    public static EnemyNew CreateEnemy(EnemyConfigData config)
    {
        GameObject enmeyIns = ResManager.instance.CreatInstance(PathConfig.EnemyPB);
        EnemyNew enmeySC = enmeyIns.GetComponent<EnemyNew>();
        // 生成 Data
        EnemyData data = new EnemyData(config.ID, config.HP, config.ShieldConfig);
        // Controller 绑定
        enmeySC.BindData(data);
        return enmeySC;
    }
    
    public static EnemyNew CreateEnemy(EnemyConfigData config,Transform parent)
    {
        EnemyNew enmeySC = CreateEnemy(config);
        enmeySC.transform.SetParent(parent,false);
        return enmeySC;
    }
    
    public static Sprite GetEnemyPortrait(int id) => 
        ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetEnemyPortrait(id));
}