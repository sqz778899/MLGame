using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RollManager: ScriptableObject
{
    #region 单例
    static RollManager s_instance;
    public static RollManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<RollManager>(PathConfig.RollManagerOBJ);
            return s_instance;
        }
    }
    #endregion

    #region SomeFunc
    public List<float> NormalizeProb(List<float> Probs)
    {
        // 1. 计算所有概率的总和
        float total = Probs.Sum();
        // 2. 如果总和为 0，返回 null 或其他适当值
        if (total == 0) return null;
        // 3. 计算新的比例，确保总和为 max
        float newRatio = 100f / total;
        // 4. 初始化列表，开始累计概率
        float start = 0f;
        List<float> normalProbs = new List<float>(Probs.Count);
        // 5. 计算归一化后的累计概率
        foreach (var prob in Probs)
        {
            float newP = prob * newRatio;
            start += newP;
            normalProbs.Add(start);
        }
        return normalProbs;
    }
    
    //................把概率重新分布.................
    public List<RollPR> DealProb(List<RollPR> OriginProbs)
    {
        List<float> orProb = new List<float>();
        foreach (var each in OriginProbs)
            orProb.Add(each.Probability);
        List<float> normalizeProb = NormalizeProb(orProb);

        List<RollPR> TargetProbs = new List<RollPR>();
        for (int i = 0; i < OriginProbs.Count; i++)
        {
            RollPR targetProb = new RollPR(OriginProbs[i].ID, normalizeProb[i]);
            TargetProbs.Add(targetProb);
        }
        
        return TargetProbs;
    }
    
    //伪随机抽取
    public RollPR SingleRoll(List<RollPR> rollProbs)
    {
        // 先计算总概率（用于归一化）
        float totalAdjustedProb = 0f;
        foreach (var rp in rollProbs)
            totalAdjustedProb += Mathf.Min(100f, rp.Probability * rp.FailCount);

        // roll一个数，决定抽到哪个
        float c = Random.Range(0f, totalAdjustedProb);
        float accum = 0f;

        RollPR selectedProb = null;

        foreach (var rp in rollProbs)
        {
            float adjustedProb = Mathf.Min(100f, rp.Probability * rp.FailCount);
            accum += adjustedProb;

            if (c <= accum)
            {
                selectedProb = rp;
                break;
            }
        }

        //更新失败次数
        foreach (var rp in rollProbs)
        {
            if (rp == selectedProb)
                rp.FailCount = 1; // 抽到后重置
            else
                rp.FailCount += 1; // 没抽到则增加失败计数
        }

        return selectedProb;
    }
    #endregion
}