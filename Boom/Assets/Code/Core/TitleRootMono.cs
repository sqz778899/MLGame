using UnityEngine;

public class TitleRootMono : MonoBehaviour
{
   public GameObject TitleGold;
   public GameObject G_SideBar;
   public GameObject G_CurBulletIcon;//侧边栏当前子弹图标
   public GameObject G_StandbyIcon;//侧边栏待机图标
   public GameObject TooltipsRoot;
   public GameObject RightClickMenuRoot;
   public GameObject StandeByRoot;
   [Header("重要资产")]
   public GUIBase SettingLv1;   //第一级Setting页面
   public SettingMono SettingSC;   //第二级Setting页面
   
   public void ChangeIcon()
   {
      G_CurBulletIcon.SetActive(!G_CurBulletIcon.activeSelf);
      G_StandbyIcon.SetActive(!G_StandbyIcon.activeSelf);
   }

   void Update()
   {
      //按ESC键，可以退出Setting界面
      if (Input.GetKey(KeyCode.Escape))
      {
         SettingSC.CloseWindow();
         SettingLv1.CloseWindow();
      }
   }

   public void ExitGame()
   {
      MSceneManager.Instance.ExitGame();
   }
}
