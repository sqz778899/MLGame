using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class UpgradeSuccessPopUI : MonoBehaviour
{
    [Header("依赖资产")]
    public SkeletonGraphic AfterSkeleton;
    public TextMeshProUGUI txtAfterName;
    public UIPopAnimator _PopAnimator;
    
    public void InitData(UpgradeBulletInfo curInfo)
    {
        //
        BulletJson afterBulletJson = null;
        if (curInfo.IsCanUpgrade)
        {
            afterBulletJson = TrunkManager.Instance.GetBulletJson(curInfo.ID + 100);
            //Debug.Log(curInfo.ID);
            BulletFactory.SetBulletInUI(AfterSkeleton,curInfo.ID + 100);
            txtAfterName.text = Loc.Get(afterBulletJson.NameKey);
        }

        PopSelf();
    }

    public void PopSelf()
    {
        _PopAnimator.PlayShow();
        _PopAnimator.PlayHide();
    }
}
