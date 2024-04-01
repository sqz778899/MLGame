using System.Collections.Generic;
using UnityEngine;

#region 背包存档相关
public class SingleSlot
{
    public int slotID;
    public int bulletID;
    public int bulletCount;
}
public class BagData
{
    public BagData()
    {
        bagSlots = new List<SingleSlot>();
        curBullets = new List<BulletData>();
    }
    public List<SingleSlot> bagSlots;
    
    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;

    public List<BulletData> curBullets;

    public void RefreshBullets(List<BulletDataJson> BulletDesignJsons)
    {
        if (curBullets == null)
            curBullets = new List<BulletData>();
        curBullets.Clear();
        if (slotRole01 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole01));
        if (slotRole02 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole02));
        if (slotRole03 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole03));
        if (slotRole04 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole04));
        if (slotRole05 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole05));
    }

    BulletData FindBulletData(List<BulletDataJson> BulletDesignJsons,int ID)
    {
        BulletData curBulletData = new BulletData();
        bool isFind = false;
        foreach (var each in BulletDesignJsons)
        {
            if (each.ID == ID)
            {
                isFind = true;
                curBulletData.ID = ID;
                curBulletData.SetDataByID();
                break;
            }
        }

        if (isFind)
            return curBulletData;
        else
            return null;
    }
    public void ClearBagData()
    {
        foreach (var each in bagSlots)
        {
            each.bulletID = 0;
            each.bulletCount = 0;
        }
        slotRole01 = 0;
        slotRole02 = 0;
        slotRole03 = 0;
        slotRole04 = 0;
        slotRole05 = 0;
    }
    
    #region IO
    public void InitDataByJson(BagDataJson BagJson,List<BulletDataJson> BulletDesignJsons)
    {
        bagSlots = BagJson.bagSlots;
        slotRole01 = BagJson.slotRole01;
        slotRole02 = BagJson.slotRole02;
        slotRole03 = BagJson.slotRole03;
        slotRole04 = BagJson.slotRole04;
        slotRole05 = BagJson.slotRole05;

        GameObject groupBulletSlot = GameObject.Find("GroupBulletSlot");
        GameObject groupBulletSlotRole = GameObject.Find("imBulletSlotRole");
        GameObject groupBullet = GameObject.Find("GroupBullet");

        BulletSlot[] allSlots = groupBulletSlot.GetComponentsInChildren<BulletSlot>();

        //实例化bullet
        foreach (SingleSlot each in bagSlots)
        {
            if (each.bulletID != 0)
            {
                GameObject bagSlotBullet = BulletManager.Instance.
                    InstanceBullet(each.bulletID,BulletInsMode.Spawner);
                DraggableBulletSpawner perSc = bagSlotBullet.GetComponentInChildren<DraggableBulletSpawner>();
                perSc.Count = each.bulletCount;
                perSc.InitData();
                
                GameObject curSlot = null;
                for (int i = 0; i < allSlots.Length; i++)
                {
                    BulletSlot curSC = allSlots[i];
                    if (curSC.SlotID == each.slotID)
                        curSlot = curSC.gameObject;
                }

                if (curSlot != null)
                {
                    bagSlotBullet.transform.SetParent(curSlot.transform,false);
                    bagSlotBullet.transform.localScale = Vector3.one;
                }
            }
        }

        //实例化bullet
        if (slotRole01 != 0)
            _instanceslotRole(slotRole01,1,BulletEditMode.SlotRole01);
        if (slotRole02 != 0)
            _instanceslotRole(slotRole02,2,BulletEditMode.SlotRole02);
        if (slotRole03 != 0)
            _instanceslotRole(slotRole03,3,BulletEditMode.SlotRole03);
        if (slotRole04 != 0)
            _instanceslotRole(slotRole04,4,BulletEditMode.SlotRole04);
        if (slotRole05 != 0)
            _instanceslotRole(slotRole05,5,BulletEditMode.SlotRole05);

        void _instanceslotRole(int slotRoleID,int slotIndex,BulletEditMode bulletEditMode)
        {
            //
            GameObject curSlotRole = groupBulletSlotRole.transform.GetChild(slotIndex - 1).gameObject;
            BulletSlotRole curSlotRoleSC = curSlotRole.GetComponent<BulletSlotRole>();
            curSlotRoleSC.IsHaveBullet = true;
            
            GameObject slotRoleGO = BulletManager.Instance.InstanceBullet(slotRoleID,BulletInsMode.EditB);
            DraggableBullet slotRoleGOSc = slotRoleGO.GetComponentInChildren<DraggableBullet>();
            slotRoleGOSc.BulletState = bulletEditMode;
            slotRoleGO.transform.SetParent(groupBullet.transform);
            slotRoleGO.transform.localScale = Vector3.one;
            slotRoleGO.transform.position = curSlotRole.transform.position;
        }
    }

    public BagDataJson SetDataJson()
    {
        BagDataJson BagJson = new BagDataJson();
        BagJson.bagSlots = bagSlots;
        BagJson.slotRole01 = slotRole01;
        BagJson.slotRole02 = slotRole02;
        BagJson.slotRole03 = slotRole03;
        BagJson.slotRole04 = slotRole04;
        BagJson.slotRole05 = slotRole05;
        return BagJson;
    }
    #endregion
}

public class BagDataJson
{
    public List<SingleSlot> bagSlots;

    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;
}
public class SaveFileJson
{
    public BagDataJson BagData;
}
#endregion

#region Map相关
public enum MapNodeState
{
    UnLocked = 1,
    Locked = 2,
    IsFinish = 3,
}
#endregion

#region 输赢条件相关
public enum WinOrFail
{
    InLevel = 1,
    Win = 2,
    Fail = 3
}
#endregion

