using UnityEngine;

public class GUICommonManager
{
    public GameObject EffectRoot{ get; private set; } //特效根节点
   // public GameObject DialogueRoot{ get; private set; } //对话框根节点
    public GameObject StandbyRoot{ get; private set; } //备选区根节点
    
    public GameObject G_SideBar{ get; private set; } //侧边栏
    public GameObject G_CurBulletIcon{ get; private set; } //侧边栏当前子弹图标
    //public GameObject G_StandbyIcon{ get; private set; } //侧边栏待机图标
    
    public GUICommonManager()
    {
        EffectRoot = EternalCavans.Instance.EffectRoot;
        //DialogueRoot = EternalCavans.Instance.DialogueRoot;
        StandbyRoot = EternalCavans.Instance.StandbyRoot;
        G_SideBar = EternalCavans.Instance.G_SideBar;
        G_CurBulletIcon = EternalCavans.Instance.G_CurBulletIcon;
        //G_StandbyIcon = EternalCavans.Instance.G_StandbyIcon;
    }
}