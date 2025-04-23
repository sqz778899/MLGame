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
    [Header("资产")]
    public GameObject FloatingTextNode;
    public bool IsStorylineLocked = true;
    
    public void LockBuild() => IsLocked = true;
    public void UnLockBuild() => IsLocked = false;
    
    internal override void Start()
    {
        base.Start();
        _outsideLogic = FindObjectOfType<OutsideLogic>();
    }

    public virtual void OnOffBuild()
    {
        if (IsStorylineLocked)
        {
            FloatingTextFactory.CreateWorldText(
                "请推进相关剧情解锁", FloatingTextNode.transform.position + new Vector3(0, 2, 0), 
                FloatingTextType.MapHint,new Color(218f / 255f, 218f / 255f, 218f / 255f, 1f), 
                7f);
            return;
        }
        _outsideLogic._builds.ForEach(menu => { if (menu != this) menu.CloseBuild(); });
        Menu.SetActive(!Menu.activeSelf);
        //锁定地图
        if(Menu.activeSelf)
            _outsideLogic.LockedMap();
        else
            _outsideLogic.UnLockedMap();
        
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
        _outsideLogic.UnLockedMap();
    }
}
