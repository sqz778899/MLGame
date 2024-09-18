using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DesignTool
{
    public int EntryID;
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Entry")]
    void AddEntry()
    {
        List<BulletEntry> DesignEntries = TrunkManager.Instance.BulletEntryDesignJsons;
        foreach (var each in DesignEntries)
        {
            if (each.ID == EntryID && !MainRoleManager.Instance.CurBulletEntries.Contains(each))
                MainRoleManager.Instance.CurBulletEntries.Add(each);
        } 
        
        UIManager.Instance.G_Help.GetComponent<HelpMono>().InitBulletEntryDes();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Entry")]
    void ClearEntry()
    {
        MainRoleManager.Instance.CurBulletEntries.Clear();
    }
    
    [Button(ButtonSizes.Large)]
    //[ButtonGroup("Entry")]
    void ClearStandby()
    {
        foreach (var each in MainRoleManager.Instance.CurStandbyBulletMats)
        {
            each.ID = 0;
            each.InstanceID = 0;
        }

        MainRoleManager.Instance.InitStandbyBulletMats();
    }
    
    [Button(ButtonSizes.Large)]
    //[ButtonGroup("Entry")]
    void ResetAll()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Switch")]
    void SwichEdit()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchEditScene();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Switch")]
    void SwichMap()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchMapScene();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Switch")]
    void SwichFight()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchFightScene();
    }

    [Title("局内调试")]
    [Button("重置子弹",ButtonSizes.Large)]
    void SetBullet()
    {
        UIManager.Instance.RoleIns.GetComponent<RoleInner>().InitData();
    }
    
    [Button("敌人血量无限",ButtonSizes.Large)]
    [ButtonGroup("调试总功能")]
    void SetEnemyHp()
    {
        Enemy curEnemy = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>().CurEnemy;
        curEnemy.CurHP = 9999;
        curEnemy.MaxHP = 9999;
    }

    void TempAddBullet(int bulletID)
    {
        BulletReady Ins = new BulletReady(bulletID,0);
        List<BulletReady> CurBullets = MainRoleManager.Instance.CurBullets;
        if (CurBullets.Count < 5)
            CurBullets.Add(Ins);
        else
            CurBullets[0] = Ins;
        UIManager.Instance.RoleIns.GetComponent<RoleInner>().InitData();
    }
    [Button("添加黏土子弹Lv1",ButtonSizes.Large)]
    [ButtonGroup("添加黏土子弹")]
    void SetClayBulletLv1()
    {
        TempAddBullet(1);
    }
    
    [Button("添加黏土子弹Lv2",ButtonSizes.Large)]
    [ButtonGroup("添加黏土子弹")]
    void SetClayBulletLv2()
    {
        TempAddBullet(101);
    }
    
    [Button("添加黏土子弹Lv3",ButtonSizes.Large)]
    [ButtonGroup("添加黏土子弹")]
    void SetClayBulletLv3()
    {
        TempAddBullet(102);
    }
    
    
    [Button("添加冰冻子弹Lv1",ButtonSizes.Large)]
    [ButtonGroup("添加冰冻子弹")]
    void SetIceBulletLv1()
    {
        TempAddBullet(2);
    }
    
    [Button("添加冰冻子弹Lv2",ButtonSizes.Large)]
    [ButtonGroup("添加冰冻子弹")]
    void SetIceBulletLv2()
    {
        TempAddBullet(102);
    }
    
    [Button("添加冰冻子弹Lv3",ButtonSizes.Large)]
    [ButtonGroup("添加冰冻子弹")]
    void SetIceBulletLv3()
    {
        TempAddBullet(202);
    }
    
    [Button("添加火焰子弹Lv1",ButtonSizes.Large)]
    [ButtonGroup("添加火焰子弹")]
    void SetFireBulletLv1()
    {
        TempAddBullet(3);
    }
    
    [Button("添加火焰子弹Lv2",ButtonSizes.Large)]
    [ButtonGroup("添加火焰子弹")]
    void SetFireBulletLv2()
    {
        TempAddBullet(103);
    }
    [Button("添加火焰子弹Lv3",ButtonSizes.Large)]
    [ButtonGroup("添加火焰子弹")]
    void SetFireBulletLv3()
    {
        TempAddBullet(203);
    }
}