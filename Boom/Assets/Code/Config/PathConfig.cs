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
   public static string RoleDesignJson = GetDataPrepath() + "Data/RoleDesign.json";
   public static string PREventDesignJson = GetDataPrepath() + "Data/PREventDesign.json";
   public static string BulletEntryDesignJson = GetDataPrepath() + "Data/BulletEntryDesign.json";
   //........................Misc...........................................
   public static string MiscDir = GetPrepath() + "Res/UI/Misc/";
   public static string FXAssetDir = GetPrepath() + "Res/FX/Prefab/";

   //.........................ScriptObject...........................
   public static string TrunkManagerOBJ = GetPrepath() + "Res/TrunkManager.asset";
   public static string GLGameDataManagerOBJ = GetPrepath() + "Res/GlobalGameDataManager.asset";
   public static string UIManagerOBJ = GetPrepath() + "Res/Manager/UIManager.asset";
   public static string RollManagerOBJ = GetPrepath() + "Res/Manager/RollManager.asset";
   public static string BuffMannagerOBJ = GetPrepath() + "Res/Manager/BuffMannager.asset";
   public static string MSceneManagerOBJ = GetPrepath() + "Res/Manager/MSceneManager.asset";
   public static string MainRoleManagerOBJ = GetPrepath() + "Res/Character/MainRole/MainRoleManager.asset";
   public static string BulletManagerOBJ = GetPrepath() + "Res/Bullet/BulletManager.asset";
   public static string CalculateDamageManagerOBJ = GetPrepath() + "Res/Manager/CalculateDamageManager.asset";
   public static string MultiLaOBJ =  GetPrepath() + "Res/Manager/MultiLa.asset";

   ///.........................资源类.................................
   public static string TooltipAsset = GetPrepath() + "Res/UI/Prefabs/P_Tooltip_01.prefab";
   public static string TexttipAsset = GetPrepath() + "Res/UI/Prefabs/P_Tooltip_Text_02.prefab";
   public static string DrawLineAsset = GetPrepath() + "Res/Map/Prefabs/P_DrawLine_01.prefab";
   public static string TxtHitPB = GetPrepath() + "Res/UI/Prefabs/P_txtHit_01.prefab";
   public static string RollScorePB = GetPrepath() + "Res/Bullet/Prefab/P_RollScore_Template.prefab";
   public static string RewardCoinAsset = GetPrepath() + "Res/UI/Prefabs/Misc/P_RewardCoin_01.prefab";
   public static string BulletEntryPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_Entry_Template.prefab";

   ///.........................GUI.................................
   public static string ShopAsset = GetPrepath() + "Res/UI/Prefabs/P_Shop.prefab";
   public static string  PRDisplayBarPB = GetPrepath() + "Res/UI/Prefabs/Misc/PRBar_Template.prefab";
   //...........................角色.................................
   public static string GetRoleImgPath(int ID)
   {
      return GetPrepath() + "Res/Character/MainRole/T_RoleSel_" + ID.ToString("D2") + ".png";
   }
   
   //..........................子弹.............................................
   public static string BulletImageDir = GetPrepath() + "Res/Bullet/Textures/";
   public static string BulletAssetDir = GetPrepath() + "Res/Bullet/Prefab/"; 
   //..........................ScoreMat.............................................
   public static string ScoreMatImage = GetPrepath() + "Res/Bullet/Textures/T_ScoreMat_01.png";
   //..........................Buff.............................................
   public static string BuffImageDir = GetPrepath() + "Res/UI/Buff/Textures/";
   public static string BuffPB = GetPrepath() + "Res/UI/Buff/Prefabs/P_Buff_Template.prefab";
   //Assets/Res/UI/Prefabs/P_Shop.prefab
   
   //..........................事件...............................
   public static string GetREventPath(int ID,MapEventType CurType)
   {
      string IDStr = ID.ToString("D2");
      string TypeStr = CurType.ToString();
      string PBName = $"P_{TypeStr}_{IDStr}.prefab" ;
      return GetPrepath() + "Res/UI/Prefabs/REvents/" + PBName;
   }
   //..........................MapNode...............................
   public static string MapNodeEvent = GetPrepath() + "Res/Map/Prefabs/P_NodeEvent_01.prefab";
   public static string MapShop = GetPrepath() + "Res/Map/Prefabs/P_NodeShop_01.prefab";
   public static string MapGoldPile = GetPrepath() + "Res/Map/Prefabs/P_NodeCoinsPile_01.prefab";
   public static string MapTreasureBox = GetPrepath() + "Res/Map/Prefabs/P_NodeTreasureBox_01.prefab";
   
   //..........................全局大关卡...............................
   public static string LevelAssetDir = GetPrepath() + "Res/Levels/";
   
   #region Function
   public static string GetFondPath(MultiLaEN MultiLa)
   {
      return GetPrepath() + string.Format("Res/UI/Fond/{0}.asset",MultiLa.ToString());
   }
   
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
            orginName = "P_Bullet_Edit_Template.prefab";
            break;
         case BulletInsMode.EditB:
            orginName = "P_Bullet_Edit_Template.prefab";
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
            orginName = "T_Bullet_Edit_b_";
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
         case BulletInsMode.Icon:
            orginName = "T_Bullet_Icon_";
            break;
         case BulletInsMode.Mat:
            orginName = "T_BulletMat_";
            break;
         //T_Bullet_Icon_01
      }

      string curIDStr = ID.ToString("D2");
      string smallDir = $"Bullet_{curIDStr}/";
      orginName = orginName + ID.ToString("D2") + ".png";
      string curImagePath = BulletImageDir + smallDir + orginName;

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
