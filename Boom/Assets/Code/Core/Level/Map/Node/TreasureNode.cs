using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TreasureNode:MapNodeBase
{
    public float UIFadeTime = 0.3f;
    public void OpenTreasureBox()
    {
        State = MapNodeState.IsFinish;
        ChangeState();

        GameObject curGBEIns = ResManager.instance.CreatInstance(PathConfig.GetBulletEntryPB);
        curGBEIns.transform.SetParent(UIManager.Instance.RewardRoot.transform,false);
        //.........................UI淡入淡出动画..................
        RectTransform curRect = curGBEIns.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(curRect, Input.mousePosition, Camera.main,out Vector3 worldPoint);
        curGBEIns.transform.position = worldPoint;
        curRect.DOMove(new Vector3(0,0,worldPoint.z),UIFadeTime);
        
        GetBEMono curSC = curGBEIns.GetComponent<GetBEMono>();
        curSC.CurTreasureNode = this;
        curSC.RollAnEntry();
        //GetBulletEntryPB
        UIManager.Instance.SetOtherUIPause();
        Debug.Log("Open Tressure Box !!");
    }
}