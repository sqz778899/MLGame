using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DesignTool
{
    #region 存档测试
    [Title("存档测试")]
    [Button("默认存档",ButtonSizes.Large),PropertyOrder(0)]
    void ResetAll()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
    }
    
    [Button("存档",ButtonSizes.Large),PropertyOrder(0)]
    [ButtonGroup("存档")]
    void SaveFile()
    {
        TrunkManager.Instance.SaveFile();
    }
    
    [Button("读档",ButtonSizes.Large),PropertyOrder(0)]
    [ButtonGroup("存档")]
    void LoadFile()
    {
        TrunkManager.Instance.LoadSaveFile();
    }
    #endregion
    
    #region 词条测试
    [Title("词条测试")]
    [PropertyOrder(1)]
    public int EntryID;
    [Button(ButtonSizes.Large),PropertyOrder(1)]
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
    
    [Button(ButtonSizes.Large),PropertyOrder(1)]
    [ButtonGroup("Entry")]
    void ClearEntry()
    {
        MainRoleManager.Instance.CurBulletEntries.Clear();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(1)]
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
    #endregion

    #region 切换场景
    [Title("切换场景")] [PropertyOrder(2)]  public int swith;
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("Switch")]
    void SwichEdit()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchEditScene();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("Switch")]
    void SwichMap()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchMapScene();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("Switch")]
    void SwichFight()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchFightScene();
    }
    #endregion

    #region 局内调试
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
        curEnemy.MaxHP = 9999;
        curEnemy.CurHP = 9999;
    }

    void TempAddBullet(int bulletID)
    {
        if (MainRoleManager.Instance.CurBullets.Count >= 5)
            MainRoleManager.Instance.CurBullets.RemoveAt(0);
        MainRoleManager.Instance.RefreshCurBullets(MutMode.Add, bulletID);
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
        TempAddBullet(201);
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
    #endregion

    [Title("道具测试")]
    [PropertyOrder(100)]
    public int ItemID;
    [Button("获得道具",ButtonSizes.Large),PropertyOrder(111)]
    void AddItem()
    {
        MainRoleManager.Instance.AddItem(ItemID);
    }
    [Title("宝石测试")]
    [PropertyOrder(101)]
    public int GemID;
    [Button("获得宝石",ButtonSizes.Large),PropertyOrder(111)]
    void AddGem()
    {
        MainRoleManager.Instance.AddGem(GemID);
    }
}