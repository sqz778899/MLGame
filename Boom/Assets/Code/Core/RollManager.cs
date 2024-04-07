using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;
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
    
    //90概率 Score
    //10概率 Bullet

    GameObject GroupRoll;
    
    public void RollBullet()
    {
        if (GroupRoll == null)
            GroupRoll = GameObject.Find("GroupRoll");
        
        //GetProbabilitys
        List<RollProbability> rollProbs = DealProb(TrunkManager.Instance.GetRollProbability());
        
        //Cal gold
        int curCost = CharacterManager.Instance.Cost;
        int curGold = CharacterManager.Instance.Gold;
        /*if (curGold < curCost)
            return;

        CharacterManager.Instance.Gold -= curCost;*/
        //Clean Ins
        //New Ins
        for (int i = 0; i < 5; i++)
        {
            RollProbability curProb = SingleRoll(rollProbs);
            
            if (curProb == null)
            {
                Debug.LogError("Roll Errro");
                return;
            }

            GameObject curRollIns = null;
            if (curProb.ID == 0)
                curRollIns = InstanceRollScore();
            else
                curRollIns = BulletManager.Instance.InstanceBullet(curProb.ID,BulletInsMode.Roll);
            curRollIns.transform.SetParent(GroupRoll.transform);
            curRollIns.transform.position = Vector3.zero;
            curRollIns.transform.localScale = Vector3.one;
            Vector3 pos = RollLayout.InsOriginPos;
            pos = new Vector3(pos.x + RollLayout.xOffset * i, pos.y, pos.z);
            curRollIns.GetComponent<RectTransform>().anchoredPosition3D = pos;
        }
    }

    List<RollProbability>  DealProb(List<RollProbability> OriginProbs)
    {
        List<RollProbability> newProbs = new List<RollProbability>();
        float start = 0f;
        float max = 100f;
        float account = 0;
        int pCount = OriginProbs.Count;
        for (int i = 0; i < pCount; i++)
        {
            account += OriginProbs[i].Probability;
        }

        if (account == 0)
            return null;
        float newRatio = 1/(account / max);
        for (int i = 0; i < pCount; i++)
        {
           float newP = OriginProbs[i].Probability * newRatio;
           start += newP;
           RollProbability curP = new RollProbability();
           curP.ID = OriginProbs[i].ID;
           curP.Probability = start;
           newProbs.Add(curP);
        }

        return newProbs;
    }

    RollProbability SingleRoll(List<RollProbability> rollProbs)
    {
        float c = Random.Range(0f, 100f);
        RollProbability curProb = null;
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

    GameObject InstanceRollScore()
    {
        GameObject RollScoreIns = Instantiate(
            ResManager.instance.GetAssetCache<GameObject>(PathConfig.RollScorePB));
        return RollScoreIns;
    }
}

