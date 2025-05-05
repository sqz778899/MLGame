using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutsideLogic : MonoBehaviour
{
    public GameObject BuildRoot;
    public QusetRoot QuestRoot;
    public MapMouseControlMainEnv MapControl;
    public List<BuildBase> _builds;

    void Awake()
    {
        _builds = new List<BuildBase>();
        for (int i = 0; i < BuildRoot.transform.childCount; i++)
            _builds.Add(BuildRoot.transform.GetChild(i).GetComponent<BuildBase>());
        
        EternalCavans.Instance.InMainEnv();
        EternalCavans.Instance.OnOpenBag += LockedAllThings;
        EternalCavans.Instance.OnCloseBag += UnLockedAllThings;
    }

    void Start()
    {
        QuestRoot.InitAllQuests();
        GM.Root.HotkeyMgr.OnEscapePressed += CloseBuild; //注册快捷键
    }

    public void LockedMap() => MapControl.LockMap();
    public void UnLockedMap() => MapControl.UnLockMap();
    
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

    void CloseBuild()
    {
        _builds.ForEach(menu => menu.CloseBuild());
        UnLockedMap();
    }

    void OnDestroy()
    {
        EternalCavans.Instance.OnOpenBag -= LockedAllThings;
        EternalCavans.Instance.OnCloseBag -= UnLockedAllThings;
        GM.Root.HotkeyMgr.OnEscapePressed -= CloseBuild;
    }
}
