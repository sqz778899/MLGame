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

    #region RollBuff
    public void OnceRollBuff()
    {
        GameObject curRoot = UIManager.Instance.G_Buff;
        int curLevelID = MainRoleManager.Instance.CurMapSate.CurLevelID;
        List<LevelBuff> curDesign = TrunkManager.Instance.LevelBuffDesignJsons;
        LevelBuff curLB = null;
        foreach (var each in curDesign)
        {
            if (each.LevelID == curLevelID)
            {
                curLB = each;
                break;
            }
        }

        if (curLB == null) return;

        List<RollPR> curBuffPool = curLB.CurBuffProb;
        int xOffset = 1167;
        int start = -1167;
        for (int i = 0; i < 3; i++)
        {
            RollPR curRoll = SingleRoll(curBuffPool);
            curBuffPool.Remove(curRoll);
            //DealProb(ref curBuffPool);
            
            GameObject curBuffPBIns = Instantiate(ResManager.
                instance.GetAssetCache<GameObject>(PathConfig.TalentPB));
            TalentMono curSC = curBuffPBIns.GetComponentInChildren<TalentMono>();
            curSC.ID = curRoll.ID;
            curSC.InitBuffData();
            curBuffPBIns.transform.SetParent(curRoot.transform);
            curBuffPBIns.transform.localScale = Vector3.one;
            curBuffPBIns.GetComponent<RectTransform>().anchoredPosition3D =
                new Vector3(start + xOffset * i, 0, 0);
        }
    }

    public void SelOneBuff(GameObject curBuffIns)
    {
        //Clean Buff
        GameObject curRoot = UIManager.Instance.G_Buff;
        for (int i = curRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(curRoot.transform.GetChild(i).gameObject);
        }
        Debug.Log("hhhh: ");
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
    
    public RollPR SingleRoll(List<RollPR> rollProbs)
    {
        float c = Random.Range(0f, 100f);
        RollPR curProb = new RollPR();
        for (int j = 0; j < rollProbs.Count; j++)
        {
            float min = 0;
            float max = 0;
            if (j == 0)
            {
                min = 0;
                max = rollProbs[j].Probability;
            }
            else
            {
                min = rollProbs[j-1].Probability;
                max = rollProbs[j].Probability;
            }

            if (c >= min && c <= max)
            {
                curProb = rollProbs[j];
                break;
            }
        }

        return curProb;
    }
    #endregion
}