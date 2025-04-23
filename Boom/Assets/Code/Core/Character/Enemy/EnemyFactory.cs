using UnityEngine;

public static class EnemyFactory
{
    public static Enemy CreateEnemy(EnemyConfigData config)
    {
        GameObject enmeyIns = ResManager.instance.CreatInstance(PathConfig.EnemyPB);
        Enemy enmeySC = enmeyIns.GetComponent<Enemy>();
        // 生成 Data
        EnemyData data = new EnemyData(config.ID, config.HP, config.ShieldConfig);
        // Controller 绑定
        enmeySC.BindData(data);
        return enmeySC;
    }
    
    public static Enemy CreateEnemy(EnemyConfigData config,Transform parent)
    {
        Enemy enmeySC = CreateEnemy(config);
        enmeySC.transform.SetParent(parent,false);
        return enmeySC;
    }
    
    public static Sprite GetEnemyPortrait(int id) => 
        ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetEnemyPortrait(id));
}