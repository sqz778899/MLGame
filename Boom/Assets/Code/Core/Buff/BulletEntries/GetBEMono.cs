using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GetBEMono:MonoBehaviour
{
    public TextMeshProUGUI CurEntryTile;
    public TreasureNode CurTreasureNode;
    
    BulletEntry curBE;
    
    public void GetEntry()
    {
        MainRoleManager.Instance.AddEntry(curBE.ID);
        CurTreasureNode.QuitNode();
        DestroyImmediate(this.gameObject);
    }

    public void RefreshEntry()
    {
        int curCost = MainRoleManager.Instance.RollEntryCost;
        if (MainRoleManager.Instance.Gold < curCost)
            return;
        
        MainRoleManager.Instance.Gold -= curCost;
        RollAnEntry(curBE); //保证刷新的词条和上一个词条不一样
    }

    public void RollAnEntry(BulletEntry Except = null)
    {
        List<BulletEntry> CurRollEntry = new List<BulletEntry>();

        List<BulletEntry> DesignBE = TrunkManager.Instance.BulletEntryDesignJsons;
        List<BulletEntry> CurBEs = MainRoleManager.Instance.CurBulletEntries;
        foreach (BulletEntry each in DesignBE)
        {
            if (!ComFunc.ContainByID(CurBEs, each))
                CurRollEntry.Add(each);
        }

        if (Except != null)
            ComFunc.RemoveByID(ref CurRollEntry, Except);

        if (CurRollEntry.Count > 0)
        {
            int curRanIndex = Random.Range(0, CurRollEntry.Count);
            curBE = CurRollEntry[curRanIndex];
            CurEntryTile.text = curBE.Name;
        }
        else 
            Debug.LogError("Non Entry Award");
    }
}