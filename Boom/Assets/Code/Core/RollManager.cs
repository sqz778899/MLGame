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

    #region Design
    //.........temp
    //90概率 Score
    //10概率 Bullet
    Vector2Int ScoreRange = new Vector2Int(3, 9);
    int Cost = 5;
    #endregion

    public bool ShopActive = false;
    
    public void OnceRollBullet()
    {
        //GetProbabilitys
        List<RollProbability> rollProbs = DealProb(TrunkManager.Instance.GetRollProbability());
        
        //Cal gold
        int curCost = CharacterManager.Instance.Cost;
        int curGold = CharacterManager.Instance.Gold;
        if (curGold < curCost)
            return;

        CharacterManager.Instance.Gold -= curCost;
        //Clean Ins
        int preRollIns = UIManager.Instance.GroupRoll.transform.childCount;
        for (int i = preRollIns - 1; i >= 0; i--)
            DestroyImmediate(UIManager.Instance.GroupRoll.transform.GetChild(i).gameObject);
        
        //New Ins
        for (int i = 0; i < 5; i++)
        {
            RollProbability curProb = SingleRoll(rollProbs);
            
            if (curProb == null)
            {
                Debug.LogError("Roll Errro");
                return;
            }

            //GetIns
            GameObject curRollIns = null;
            if (curProb.ID == 0)
                curRollIns = InstanceRollScore();
            else
                curRollIns = BulletManager.Instance.InstanceBullet(curProb.ID,BulletInsMode.Roll);
            SetRollAttri(curRollIns, i);
        }
        
        TrunkManager.Instance.SaveFile();
    }

    public void SelOne(GameObject SelGO)
    {
        RollBullet curSC = SelGO.GetComponentInChildren<RollBullet>();
        //............Cost Money.................
        int curCost = curSC.Cost;
        if (CharacterManager.Instance.Gold < curCost)
        {
            Debug.Log("No Money");
            return;
        }
        CharacterManager.Instance.Gold -= curCost;
        
        //............Deal Data.................
        if (curSC._bulletData.ID == 0)//Score
        {
            CharacterManager.Instance.Score +=  curSC.Score;
        }
        else
        {
            bool isAdd = CharacterManager.Instance.AddStandbyBullet(curSC._bulletData.ID);
            if (!isAdd)
            {
                Debug.Log("qweqwesxas");
                return;
            }

            BulletManager.Instance.BulletUpgrade();
        }
        TrunkManager.Instance.SaveFile();
    }
    
    public void OnOffShop()
    {
        GameObject shop = UIManager.Instance.ShopCanvas;
        ShopActive = shop.transform.GetChild(0).gameObject.activeSelf;
        for (int i = 0; i < shop.transform.childCount; i++)
            shop.transform.GetChild(i).gameObject.SetActive(!ShopActive);
        ShopActive = !ShopActive;
    }

    #region SomeFunc
    void SetRollAttri(GameObject curRollIns,int step)
    {
        //..................POS............................
        curRollIns.transform.SetParent(UIManager.Instance.GroupRoll.transform);
        curRollIns.transform.position = Vector3.zero;
        curRollIns.transform.localScale = Vector3.one;
        Vector3 pos = RollLayout.InsOriginPos;
        pos = new Vector3(pos.x + RollLayout.xOffset * step, pos.y, pos.z);
        curRollIns.GetComponent<RectTransform>().anchoredPosition3D = pos;
        //...............Attri.............................................
        RollBullet curSC = curRollIns.GetComponentInChildren<RollBullet>();
        int curScore = Random.Range(ScoreRange.x, ScoreRange.y+1);
        curSC.Score = curScore;
        curSC.Cost = Cost;
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
    #endregion
}