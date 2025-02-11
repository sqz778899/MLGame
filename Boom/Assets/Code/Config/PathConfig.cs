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
   public static string ItemDesignJson = GetDataPrepath() + "Data/ItemDesign.json";
   public static string GemDesignJson = GetDataPrepath() + "Data/GemDesign.json";
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
   public static string TexttipAsset = GetPrepath() + "Res/UI/Prefabs/Misc/P_Tooltip_Text_02.prefab";
   public static string DrawLineAsset = GetPrepath() + "Res/Map/Prefabs/P_DrawLine_01.prefab";
   public static string TxtHitPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_txtHit_01.prefab";
   public static string RollScorePB = GetPrepath() + "Res/Bullet/Prefab/P_RollScore_Template.prefab";
   public static string RewardCoinAsset = GetPrepath() + "Res/UI/Prefabs/Misc/P_RewardCoin_01.prefab";
   public static string BulletEntryPB = GetPrepath() + "Res/UI/Buff/Entry/P_Entry_Template.prefab";
   public static string GetBulletEntryPB = GetPrepath() + "Res/UI/Prefabs/Popup/P_GetBulletEntry_Template.prefab";
   public static string ItemImageDir = GetPrepath() + "Res/UI/Item/Textures/";
   public static string GemImageDir = GetPrepath() + "Res/UI/Gem/Textures/";
   public static string ItemPB = GetPrepath() + "Res/UI/Item/Prefabs/P_Item_Template_01.prefab";

   ///.........................GUI.................................
   public static string ShopAsset = GetPrepath() + "Res/UI/Prefabs/Popup/P_Shop.prefab";
   public static string  PRDisplayBarPB = GetPrepath() + "Res/UI/Prefabs/Misc/PRBar_Template.prefab";
   public static string  BulletUPPB = GetPrepath() + "Res/UI/Prefabs/Popup/P_BulletUP_01.prefab";
   public static string RightClickMenu = GetPrepath() + "Res/UI/Prefabs/RightClickMenu.prefab";
   public static string AwardTextPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_Award_01.prefab";
   public static string AwardCoin = GetPrepath() + "Res/UI/Prefabs/Misc/P_Award_Coin_01.prefab";
   public static string AwardRoomKey = GetPrepath() + "Res/UI/Prefabs/Misc/P_Award_RoomKey_01.prefab";
   public static string DialogueFightPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_Dialogue_Fight_Template.prefab";
   
   //...........................角色.................................
   public static string ShieldPB = GetPrepath() + "Res/Character/Image/Enemy/P_Shield_01.prefab";
   public static string GetRoleImgPath(int ID)
   {
      return GetPrepath() + "Res/Character/MainRole/T_RoleSel_" + ID.ToString("D2") + ".png";
   }
   
   //..........................子弹.............................................
   public static string BulletImageDir = GetPrepath() + "Res/Bullet/Textures/";
   public static string BulletSpineDir = GetPrepath() + "Res/Bullet/SpineData/";
   public static string BulletAssetDir = GetPrepath() + "Res/Bullet/Prefab/"; 
   public static string BulletSpfxTemplate = GetPrepath() + "Res/Bullet/Prefab/P_Bullet_Inner_Spfx_Template.prefab";
   
   //...........................Gem.............................................
   public static string GemTemplate = GetPrepath() + "Res/UI/Gem/Prefabs/P_Gem_Template_01.prefab";
   
   //..........................ScoreMat.........................................
   public static string ScoreMatImage = GetPrepath() + "Res/Bullet/Textures/T_ScoreMat_01.png";
   //..........................Buff.............................................
   public static string TalentImageDir = GetPrepath() + "Res/UI/Buff/Talent/Textures/";
   public static string TalentPB = GetPrepath() + "Res/UI/Buff/Talent/P_Buff_Template.prefab";
   //..........................Connon...........................................
   public static string ConnonPB = GetPrepath() + "Res/Character/SpineData/Connon01/P_Connon_01.prefab";
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
   
   //..........................通用材质球..............................
   public static string MatOutLine = GetPrepath() + "Res/Shader/CommonMaterial/OutLine.mat";
   
   #region Function
   //获得关卡的路径
   public static string GetLevelPath(int levelID)
   {
      return LevelAssetDir + $"P_Level_{levelID.ToString("00")}.prefab";
   }
   //获得宝石资产的路径
   public static string GetItemPath(string name)
   {
      return ItemImageDir + $"{name}.png";
   }
   //获得宝石资产的路径
   public static string GetGemPath(string name)
   {
      return GemImageDir + $"{name}.png";
   }
   
   //获得字体的路径
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
         case BulletInsMode.Mat:
            orginName = "P_BulletRollMat_Template.prefab";
            break;
         case BulletInsMode.Standby:
            orginName = "P_StandbyMat_Template.prefab";
            break;
      }
      string BulletTemplate = BulletAssetDir + orginName;
      return BulletTemplate;
   }

   public static string GetBulletImageOrSpinePath(int ID,BulletInsMode bulletInsMode)
   {
      string orginName = "";
      string suffix = ".png";
      string curDir = BulletImageDir;
      switch (bulletInsMode)
      {
         case BulletInsMode.Inner:
            curDir = BulletSpineDir;
            suffix = ".asset";
            orginName = "bullet_";//bullet_001_SkeletonData
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

      int orignalID = ID % 10;
      string smallDir = $"Bullet_{orignalID.ToString("D3")}/";
      if (suffix == ".asset")
         orginName = $"{orginName}{ID.ToString("D3")}_SkeletonData{suffix}";
      else
         orginName = $"{orginName}{ID.ToString("D3")}{suffix}";
      
      string curImagePath = curDir + smallDir + orginName;

      return curImagePath;
   }
   
   //spfx_hit_001_SkeletonData
   public static string GetBulletSpfxPath(int ID)
   {
      int orignalID = ID % 10;
      string smallDir = $"Bullet_{orignalID.ToString("D3")}/";
      string curDir = BulletSpineDir;
      string finnal = $"{curDir}{smallDir}spfx_hit_{ID.ToString("D3")}_SkeletonData.asset";
      return finnal;
   }

   public static string GetBufftImagePath(int ID,string name)
   {
      string orginName = "T_Buff_" + name +"_" +  ID.ToString("D2") + ".png";
      string curImagePath = TalentImageDir + orginName;
      
      return curImagePath;
   }
   #endregion
}
