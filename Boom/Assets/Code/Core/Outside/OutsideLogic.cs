using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutsideLogic : MonoBehaviour
{
    public GameObject BuildRoot;
    public MapMouseControlMainEnv MapControl;
    public List<BuildBase> _builds;

    void Awake()
    {
        _builds = new List<BuildBase>();
        for (int i = 0; i < BuildRoot.transform.childCount; i++)
            _builds.Add(BuildRoot.transform.GetChild(i).GetComponent<BuildBase>());
        
        //todo 单独启动场景时候的初始化
        TrunkManager.Instance.ForceRefresh();
        UIManager.Instance.InitStartGame();
        SaveManager.LoadSaveFile();
        MainRoleManager.Instance.InitData();
        //todo ......................
        EternalCavans.Instance.InMainEnv();
        EternalCavans.Instance.OnOpenBag += LockedAllThings;
        EternalCavans.Instance.OnCloseBag += UnLockedAllThings;
    }
    
    void LockedAllThings()
    {
        MapControl.LockMap();
        _builds.ForEach(s=>s.LockBuild());
    }
    
    void UnLockedAllThings()
    {
        MapControl.UnLockMap();
        _builds.ForEach(s=>s.UnLockBuild());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _builds.ForEach(menu => menu.CloseBuild());
        }
    }

    void OnDestroy()
    {
        EternalCavans.Instance.OnOpenBag -= LockedAllThings;
        EternalCavans.Instance.OnCloseBag -= UnLockedAllThings;
    }
}
