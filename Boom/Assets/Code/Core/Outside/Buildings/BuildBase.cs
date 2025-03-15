using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBase : SpriteClickHandler
{
    [Header("点击后是否关闭高亮")]
    public bool IsQuitHighLight = true;
    public GameObject Menu;
    OutsideLogic _outsideLogic;
    [Header("冲突锁定的建筑")] 
    public List<SpriteClickHandler> NeedLockedBuilds;
    
    public void LockBuild() => IsLocked = true;
    public void UnLockBuild() => IsLocked = false;
    
    internal override void Start()
    {
        base.Start();
        _outsideLogic = FindObjectOfType<OutsideLogic>();
    }

    public virtual void OnOffBuild()
    {
        _outsideLogic._builds.ForEach(menu => { if (menu != this) menu.CloseBuild(); });
        Menu.SetActive(!Menu.activeSelf);
        if (IsQuitHighLight)
            QuitHighLight();
        //因为UI摆放问题，这里需要手动设置一下
        if (Menu.activeSelf)
            NeedLockedBuilds.ForEach(needLockedBuild => needLockedBuild.IsLocked = true);
        else
            NeedLockedBuilds.ForEach(needLockedBuild => needLockedBuild.IsLocked = false);
    }

    public virtual void CloseBuild()
    {
        Menu.SetActive(false);
        NeedLockedBuilds.ForEach(needLockedBuild => needLockedBuild.IsLocked = false);
    }
}
