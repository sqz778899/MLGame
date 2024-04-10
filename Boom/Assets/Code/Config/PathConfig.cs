using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathConfig
{
   public static string MiscDir = "Assets/Res/UI/Misc/";

   
   public static string SaveFileJson = "Assets/Data/SaveFile.json";
   public static string BulletDesignJson = "Assets/Data/BulletDesign.json";
   public static string FXAssetDir = "Assets/Res/FX/Prefab/";

   //.........................ScriptObject...........................
   public static string TrunkManagerOBJ = "Assets/Res/TrunkManager.asset";
   public static string UIManagerOBJ = "Assets/Res/Manager/UIManager.asset";
   public static string RollManagerOBJ = "Assets/Res/Manager/RollManager.asset";
   public static string MSceneManagerOBJ = "Assets/Res/Manager/MSceneManager.asset";
   public static string CharacterManagerOBJ = "Assets/Res/Character/CharacterManager.asset";
   public static string BulletManagerOBJ = "Assets/Res/Bullet/BulletManager.asset";
   public static string CalculateDamageManagerOBJ = "Assets/Res/Manager/CalculateDamageManager.asset";

   ///.........................资源类.................................
   public static string DrawLineAsset = "Assets/Res/UI/Map/Prefab/P_DrawLine_01.prefab";
   public static string TxtHitPB = "Assets/Res/UI/Prefab/P_txtHit_01.prefab";
   public static string RollScorePB = "Assets/Res/Bullet/Prefab/P_RollScore_Template.prefab";
   
   //..........................子弹.............................................
   public static string BulletImageDir = "Assets/Res/Bullet/Textures/";
   public static string BulletAssetDir = "Assets/Res/Bullet/Prefab/";
   
   //..........................全局大关卡...............................
   public static string LevelAssetDir = "Assets/Res/Levels/";


   public static string GetBulletTemplate(BulletInsMode bulletInsMode)
   {
      string orginName = "";
      switch (bulletInsMode)
      {
         case BulletInsMode.Inner:
            orginName = "P_Bullet_Inner_Template.prefab";
            break;
         case BulletInsMode.Spawner:
            orginName = "P_Bullet_Spawner_Template.prefab";
            break;
         case BulletInsMode.EditA:
            orginName = "P_Bullet_Edit_a_Template.prefab";
            break;
         case BulletInsMode.EditB:
            orginName = "P_Bullet_Edit_a_Template.prefab";
            break;
         case BulletInsMode.Roll:
            orginName = "P_Bullet_Roll_Template.prefab";
            break;
         case BulletInsMode.Standby:
            orginName = "P_Bullet_Standby_Template.prefab";
            break;
      }
      string BulletTemplate = BulletAssetDir + orginName;
      return BulletTemplate;
   }

   public static string GetBulletImagePath(int ID,BulletInsMode bulletInsMode)
   {
      //temp todo
      ID = ID % 10;
      /*if ()
      {
         
      }*/
      //"T_Bullet_Edit_a_01""T_Bullet_Inner_01"
      string orginName = "";
      switch (bulletInsMode)
      {
         case BulletInsMode.Inner:
            orginName = "T_Bullet_Inner_";
            break;
         case BulletInsMode.Spawner:
            orginName = "T_Bullet_Edit_a_";
            break;
         case BulletInsMode.EditA:
            orginName = "T_Bullet_Edit_a_";
            break;
         case BulletInsMode.EditB:
            orginName = "T_Bullet_Edit_a_";
            break;
         case BulletInsMode.Roll:
            orginName = "T_Bullet_Edit_a_";
            break;
         case BulletInsMode.Standby:
            orginName = "T_Bullet_Edit_a_";
            break;
      }
      orginName = orginName + ID.ToString("D2") + ".png";
      string curImagePath = BulletImageDir + orginName;

      return curImagePath;
   }
}
