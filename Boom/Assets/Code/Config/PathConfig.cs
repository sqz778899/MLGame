using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathConfig
{
   public static string MiscDir = "Assets/Res/UI/Misc/";

   
   public static string SaveFileJson = "Assets/Data/SaveFile.json";
   public static string BulletDesignJson = "Assets/Data/BulletDesign.json";
   public static string BulletAssetDir = "Assets/Res/Bullet/Prefab/";
   
   //.........................ScriptObject...........................
   public static string MSceneManagerOBJ = "Assets/Res/MSceneManager.asset";
   public static string CharacterManagerOBJ = "Assets/Res/CharacterManager.asset";
   ///.........................资源类.................................
   public static string DrawLineAsset = "Assets/Res/UI/Map/Prefab/P_DrawLine_01.prefab";
   
   //..........................全局大关卡...............................
   public static string LevelAssetDir = "Assets/Res/Levels/";


   public static void GetBulletImageName(int ID,BulletInsMode bulletInsMode)
   {
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
      }
      orginName = orginName + ID.ToString("D2");
   }
}
