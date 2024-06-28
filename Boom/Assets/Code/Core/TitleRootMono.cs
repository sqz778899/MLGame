using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRootMono : MonoBehaviour
{
   public GameObject TitleGold;
   public GameObject G_CurBulletIcon;//侧边栏当前子弹图标
   public GameObject G_StandbyIcon;//侧边栏待机图标

   public GameObject G_Help;
   public GameObject G_Setting;
   public void ChangeIcon()
   {
      G_CurBulletIcon.SetActive(!G_CurBulletIcon.activeSelf);
      G_StandbyIcon.SetActive(!G_StandbyIcon.activeSelf);
   }
}
