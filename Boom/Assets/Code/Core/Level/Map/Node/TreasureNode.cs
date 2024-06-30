using System.Collections.Generic;
using UnityEngine;

public class TreasureNode:MapNodeBase
{
    public void OpenTreasureBox()
    {
        State = MapNodeState.IsFinish;
        ChangeState();
        
        GameObject curGBEIns = ResManager.instance.CreatInstance(PathConfig.GetBulletEntryPB);
        curGBEIns.transform.SetParent(UIManager.Instance.RewardRoot.transform,false);
        GetBEMono curSC = curGBEIns.GetComponent<GetBEMono>();
        curSC.CurTreasureNode = this;
        curSC.RollAnEntry();
        //GetBulletEntryPB
        UIManager.Instance.SetOtherUIPause();
        Debug.Log("Open Tressure Box !!");
    }
}