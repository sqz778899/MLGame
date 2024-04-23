using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathConfig
{
   public static string GetPrepath()
   {
#if UNITY_EDITOR
      return "Assets/";
#endif
      return Application.streamingAssetsPath + "/";
   }
   
   public static string GetDataPrepath()
   {
#if UNITY_EDITOR
      return "Assets/StreamingAssets/";
#endif
      return Application.streamingAssetsPath + "/";
   }
   
   //......................Data..............................................
   public static string MultiLaDesignJson = GetDataPrepath() + "Data/MultiLa.json";
   public static string SaveFileJson = GetDataPrepath() + "Data/SaveFile.json";
   public static string UserConfigJson = GetDataPrepath() + "Data/UserConfig.json";
   public static string BulletDesignJson = GetDataPrepath() + "Data/BulletDesign.json";
   public static string BuffDesignJson = GetDataPrepath() + "Data/BuffDesign.json";
   public static string LevelBuffDesignJson = GetDataPrepath() + "Data/LevelBuffDesign.json";
   //........................Misc...........................................
   public static string MiscDir = GetPrepath() + "Res/UI/Misc/";
   public static string FXAssetDir = GetPrepath() + "Res/FX/Prefab/";

   //.........................ScriptObject...........................
   public static string TrunkManagerOBJ = GetPrepath() + "Res/TrunkManager.asset";
   public static string UIManagerOBJ = GetPrepath() + "Res/Manager/UIManager.asset";
   public static string RollManagerOBJ = GetPrepath() + "Res/Manager/RollManager.asset";
   public static string BuffMannagerOBJ = GetPrepath() + "Res/Manager/BuffMannager.asset";
   public static string MSceneManagerOBJ = GetPrepath() + "Res/Manager/MSceneManager.asset";
   public static string CharacterManagerOBJ = GetPrepath() + "Res/Character/CharacterManager.asset";
   public static string BulletManagerOBJ = GetPrepath() + "Res/Bullet/BulletManager.asset";
   public static string CalculateDamageManagerOBJ = GetPrepath() + "Res/Manager/CalculateDamageManager.asset";
   public static string MultiLaOBJ =  GetPrepath() + "Res/Manager/MultiLa.asset";
   

   ///.........................资源类.................................
   public static string TooltipAsset = GetPrepath() + "Res/UI/Prefab/P_Tooltip_01.prefab";
   public static string TexttipAsset = GetPrepath() + "Res/UI/Prefab/P_Tooltip_Text_02.prefab";
   public static string DrawLineAsset = GetPrepath() + "Res/UI/Map/Prefab/P_DrawLine_01.prefab";
   public static string TxtHitPB = GetPrepath() + "Res/UI/Prefab/P_txtHit_01.prefab";
   public static string RollScorePB = GetPrepath() + "Res/Bullet/Prefab/P_RollScore_Template.prefab";

   //..........................子弹.............................................
   public static string BulletImageDir = GetPrepath() + "Res/Bullet/Textures/";
   public static string BulletAssetDir = GetPrepath() + "Res/Bullet/Prefab/"; 
   //..........................Buff.............................................
   public static string BuffImageDir = GetPrepath() + "Res/UI/Buff/Textures/";
   public static string BuffPB = GetPrepath() + "Res/UI/Buff/Prefabs/P_Buff_Template.prefab";
   
   //..........................全局大关卡...............................
   public static string LevelAssetDir = GetPrepath() + "Res/Levels/";
   

   public static string GetFondPath(MultiLaEN MultiLa)
   {
      return GetPrepath() + string.Format("Res/UI/Fond/{0}.asset",MultiLa.ToString());
   }
   
   #region Function
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
         case BulletInsMode.Thumbnail:
            orginName = "T_Bullet_Edit_a_";
            break;
      }
      orginName = orginName + ID.ToString("D2") + ".png";
      string curImagePath = BulletImageDir + orginName;

      return curImagePath;
   }

   public static string GetBufftImagePath(int ID,string name)
   {
      string orginName = "T_Buff_" + name +"_" +  ID.ToString("D2") + ".png";
      string curImagePath = BuffImageDir + orginName;
      
      return curImagePath;
   }
   #endregion
}
