using System.Collections.Generic;
using System.Linq;
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
        SaveManager.SaveFile();
    }
    
    [Button("读档",ButtonSizes.Large),PropertyOrder(0)]
    [ButtonGroup("存档")]
    void LoadFile()
    {
        SaveManager.LoadSaveFile();
    }

    [Button("强制刷新配表数据",ButtonSizes.Large),PropertyOrder(0)]
    void sss()
    {
        TrunkManager.Instance.ForceRefresh();
    }
    #endregion

    #region 切换场景
    [Title("切换场景")] [PropertyOrder(2)]  public int swith;
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("Switch")]
    void SwichBag()
    {
        UIManager.Instance.IsLockedClick = false;
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchBag();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("Switch")]
    void SwichMap()
    {
        UIManager.Instance.IsLockedClick = false;
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchMapScene();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("Switch")]
    void SwichFight()
    {
        UIManager.Instance.IsLockedClick = false;
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchFightScene();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("SwichBag")]
    void SwichBullet()
    {
        UIManager.Instance.IsLockedClick = false;
        GameObject BagRootGO = GameObject.Find("BagRoot");
        BagRootGO.GetComponent<BagRoot>().SwichBullet();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("SwichBag")]
    void SwichGem()
    {
        UIManager.Instance.IsLockedClick = false;
        GameObject BagRootGO = GameObject.Find("BagRoot");
        BagRootGO.GetComponent<BagRoot>().SwichGem();
    }
    
    [Button(ButtonSizes.Large),PropertyOrder(2)]
    [ButtonGroup("SwichBag")]
    void SwichItem()
    {
        UIManager.Instance.IsLockedClick = false;
        GameObject BagRootGO = GameObject.Find("BagRoot");
        BagRootGO.GetComponent<BagRoot>().SwichItem();
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
        Enemy curEnemy = UIManager.Instance.BattleLogicGO.GetComponent<BattleLogic>().CurEnemy;
        curEnemy.MaxHP = 9999;
        curEnemy.CurHP = 9999;
    }

    void TempAddBullet(int bulletID)
    {
        if (MainRoleManager.Instance.CurBullets.Count >= 5)
            MainRoleManager.Instance.CurBullets.RemoveAt(0);
        MainRoleManager.Instance.AddCurBullet(new BulletData(bulletID,SlotManager.GetEmptySlot(SlotType.CurBulletSlot)));
        MainRoleManager.Instance.InitCurBullets();
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

    #region 地图编辑
    [Title("雾"),PropertyOrder(98)]
    public GameObject MapRoot;
    [Button("雾关闭",ButtonSizes.Large),PropertyOrder(99)]
    [ButtonGroup("雾")]
    void CloseFog()
    {
        if (!MapRoot)
            MapRoot = GameObject.Find("MapRoot");
        MapRoomNode[] allRoom = MapRoot.GetComponentsInChildren<MapRoomNode>();

        allRoom.Where(each => each.RoomFog != null).ToList() // 将结果转换为列表
            .ForEach(each => each.RoomFog.gameObject.SetActive(false)); // 执行操作
    }
    
    [Button("雾开启",ButtonSizes.Large),PropertyOrder(99)]
    [ButtonGroup("雾")]
    void OpenFog()
    {
        if (!MapRoot)
            MapRoot = GameObject.Find("MapRoot");
        MapRoomNode[] allRoom = MapRoot.GetComponentsInChildren<MapRoomNode>();
        allRoom.Where(each => each.RoomFog != null).ToList() // 将结果转换为列表
            .ForEach(each => each.RoomFog.gameObject.SetActive(true)); // 执行操作
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