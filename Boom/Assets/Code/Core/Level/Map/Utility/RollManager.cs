using System.Collections.Generic;
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
            DealProb(ref curBuffPool);
            
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
        List<float> normalProbs = new List<float>();
        float start = 0f;
        float max = 100f;
        float account = 0;
        
        for (int i = 0; i < Probs.Count; i++)
            account += Probs[i];

        if (account == 0) return null;
        
        float newRatio = 1/(account / max);
        for (int i = 0; i < Probs.Count; i++)
        {
            float newP = Probs[i] * newRatio;
            start += newP;
            normalProbs.Add(start);
        }
        return normalProbs;
    }
    
    //................把概率重新分布.................
    public void DealProb(ref List<RollPR> OriginProbs)
    {
        List<RollPR> newProbs = new List<RollPR>();
        List<float> orProb = new List<float>();
        foreach (var each in OriginProbs)
            orProb.Add(each.Probability);
        List<float> normalizeProb = NormalizeProb(orProb);

        for (int i = 0; i < OriginProbs.Count; i++)
        {
            OriginProbs[i].Probability = normalizeProb[i];
        }
    }
    public List<RollPR> DealProb(List<RollPR> OriginProbs)
    {
        List<RollPR> newProbs = new List<RollPR>();
        List<float> orProb = new List<float>();
        foreach (var each in OriginProbs)
            orProb.Add(each.Probability);
        List<float> normalizeProb = NormalizeProb(orProb);

        for (int i = 0; i < OriginProbs.Count; i++)
        {
            RollPR tmp = new RollPR();
            tmp.ID = OriginProbs[i].ID;
            tmp.Probability = normalizeProb[i];
            newProbs.Add(tmp);
        }
        return newProbs;
    }
    
    public RollPR 
        SingleRoll(List<RollPR> rollProbs)
    {
        float c = Random.Range(0f, 100f);
        RollPR curProb = null;
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