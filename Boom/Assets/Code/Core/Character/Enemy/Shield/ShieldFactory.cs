using System.Collections.Generic;
using UnityEngine;

public static class ShieldFactory
{
    public static Shield CreateShield(ShieldData data,Transform parent = null)
    {
        GameObject shieldIns = ResManager.instance.CreatInstance(PathConfig.ShieldPB);
        if (parent != null)
            shieldIns.transform.SetParent(parent, false);
        Shield shieldSC = shieldIns.GetComponent<Shield>();
        // Controller 绑定
        shieldSC.BindData(data);
        return shieldSC;  
    }
}