using System.Collections.Generic;
using UnityEngine;

public class EnemyConfigMono : MonoBehaviour
{
    [Header("基础参数")]
    public int ID;
    public int MaxHP;

    [Header("盾牌血量（每个值代表一个盾）")]
    public List<int> ShieldsHPs = new();

    [Header("战斗掉落奖励")]
    public Award CurAward;

    // 转换成运行时数据结构
    public EnemyData ToEnemyData()
    {
        var data = new EnemyData
        {
            ID = ID,
            MaxHP = MaxHP,
            CurHP = MaxHP,
            CurAward = CurAward,
            EState = EnemyState.live
        };
        data.SetShieldData(new List<int>(ShieldsHPs));
        return data;
    }
}