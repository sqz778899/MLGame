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
   //..........................全局大关卡...............................
   public static string LevelAssetDir = GetPrepath() + "Res/Levels/";
   
   //..........................全局大地图...............................
   public static string MapPB(int ID) => GetPrepath() + $"Res/Map/P_Map_{ID.ToString("D2")}.prefab";
   
   //......................Data..............................................
   public static string MultiLaDesignJson = GetDataPrepath() + "Data/MultiLa.json";
   public static string SaveFileJson = GetDataPrepath() + "Data/SaveFile.json";
   public static string UserConfigJson = GetDataPrepath() + "Data/UserConfig.json";
   public static string BulletDesignJson = GetDataPrepath() + "Data/BulletDesign.json";
   public static string ItemDesignJson = GetDataPrepath() + "Data/ItemDesign.json";
   public static string GemDesignJson = GetDataPrepath() + "Data/GemDesign.json";
   public static string QuestDesignJson = GetDataPrepath() + "Data/QuestDesign.json";
   
   public static string DialogueDesignJson = GetDataPrepath() + "Data/DialogueDesign.json";
   public static string TalentDesignJson = GetDataPrepath() + "Data/TalentDesign.json";

   //.........................ScriptObject...........................
   public static string TrunkManagerOBJ = GetPrepath() + "Res/TrunkManager.asset";
   public static string UIManagerOBJ = GetPrepath() + "Res/Manager/UIManager.asset";
   public static string RollManagerOBJ = GetPrepath() + "Res/Manager/RollManager.asset";
   public static string MSceneManagerOBJ = GetPrepath() + "Res/Manager/MSceneManager.asset";
   public static string CalculateDamageManagerOBJ = GetPrepath() + "Res/Manager/CalculateDamageManager.asset";
   public static string MultiLaOBJ =  GetPrepath() + "Res/Manager/MultiLa.asset";
   
   public static string DialogueNamePortraitOBJ =  GetPrepath() + "Res/Manager/DialogueNamePortraitConfig.asset";
   ///.........................资源类.................................
   //各种跳字
   public static string TxtHitPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_txtHit_01.prefab";
   public static string TxtGetItemPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_TextGetItem_01.prefab";
   public static string TxtFloatingUIPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_UIFloatingtxt_01.prefab";
   //商店
   public static string RollGemPB = GetPrepath() + "Res/UI/Gem/Prefabs/P_GemInShop_Template.prefab";
   public static string ItemPersistentImageDir = GetPrepath() + "Res/UI/Item/Textures/Persistent/";
   public static string ItemEquipableImageDir = GetPrepath() + "Res/UI/Item/Textures/Equipable/";
   public static string GemImageDir = GetPrepath() + "Res/UI/Gem/Textures/";
   public static string ItemPB = GetPrepath() + "Res/UI/Item/Prefabs/P_Item_Template_01.prefab";

   ///.........................GUI.................................
   public static string ShopAsset = GetPrepath() + "Res/UI/Prefabs/Popup/P_Shop.prefab";
   public static string  BulletUPPB = GetPrepath() + "Res/UI/Prefabs/Popup/P_BulletUP_01.prefab";
   public static string AwardCoin = GetPrepath() + "Res/UI/Prefabs/Misc/P_Award_Coin_01.prefab";
   public static string AwardRoomKey = GetPrepath() + "Res/UI/Prefabs/Misc/P_Award_RoomKey_01.prefab";
   public static string DialogueFightPB = GetPrepath() + "Res/UI/Prefabs/Misc/P_Dialogue_Fight_Template.prefab";
   public static string ObserveHPPB = GetPrepath() + "Res/Character/Image/Enemy/P_ObserveHP_01.prefab";
   
   //...........................角色.................................
   public static string EnemyPB = GetPrepath() + "Res/Character/SpineData/P_Enemy_Template.prefab";
   public static string GetEnemySkelentonDataPath(int ID) =>
      GetPrepath() + $"Res/Character/SpineData/Enemy{ID.ToString("D2")}/enemy_{ID.ToString("D2")}_SkeletonData.asset";
   public static string GetEnemyPortrait(int ID) => 
      GetPrepath() + $"Res/Character/Image/Enemy/Enemy_Portrait_{ID.ToString("D2")}.png";
   public static string ShieldPB = GetPrepath() + "Res/Character/Image/Enemy/P_Shield_01.prefab";
   
   //..........................子弹.............................................
   public static string BulletImageDir = GetPrepath() + "Res/Bullet/Textures/";
   public static string BulletSpineDir = GetPrepath() + "Res/Bullet/SpineData/";
   public static string BulletAssetDir = GetPrepath() + "Res/Bullet/Prefab/"; 
   public static string BulletSpfxTemplate = GetPrepath() + "Res/Bullet/Prefab/P_Bullet_Inner_Spfx_Template.prefab";
   
   //...........................Gem.............................................
   public static string GemTemplate = GetPrepath() + "Res/UI/Gem/Prefabs/P_Gem_Template_01.prefab";
   public static string GemInnerTemplate = GetPrepath() + "Res/UI/Gem/Prefabs/P_GemInner_Template.prefab";
   
   //..........................ScoreMat.........................................
   public static string ScoreMatImage = GetPrepath() + "Res/Bullet/Textures/T_ScoreMat_01.png";
   
   //..........................Buff.............................................

   //..........................Connon...........................................
   public static string ConnonPB = GetPrepath() + "Res/Character/SpineData/Connon01/P_Connon_01.prefab";
   
   //..........................通用材质球..............................
   public static string MatOutLine = GetPrepath() + "Res/Shader/CommonMaterial/OutLine.mat";
   public static string MatUIOutLine = GetPrepath() + "Res/Shader/CommonMaterial/UIOutLine.mat";
   
   //..............................OBJ Data..............................................
   public static string PlayerDataPath = GetPrepath() + "Res/DataOBJ/PlayerData.asset";
   public static string QuestDataPath = GetPrepath() + "Res/DataOBJ/QuestData.asset";
   public static string InventoryDataPath = GetPrepath() + "Res/DataOBJ/InventoryData.asset";
   public static string BulletInvDataPath = GetPrepath() + "Res/DataOBJ/BulletInvData.asset";
   public static string BattleDataPath = GetPrepath() + "Res/DataOBJ/BattleData.asset";
   
   #region Function
   //获得关卡的路径
   public static string GetLevelPath(int levelID)
   {
      return LevelAssetDir + $"P_Level_{levelID.ToString("00")}.prefab";
   }
   //获得宝石资产的路径
   public static string GetItemPath(string name,ItemCategory Category)
   {
      string curDir = ItemPersistentImageDir;
      if (Category == ItemCategory.Equipable)
         curDir = ItemEquipableImageDir;
      return curDir + $"{name}.png";
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
         case BulletInsMode.EditInner:
            orginName = "P_BulletInner_Edit_Template.prefab";
            break;
         case BulletInsMode.Inner:
            orginName = "P_Bullet_Inner_Template.prefab";
            break;
         case BulletInsMode.Spawner:
            orginName = "P_Bullet_Spawner_Template.prefab";
            break;
         case BulletInsMode.SpawnerInner:
            orginName = "P_Bullet_SpawnerInner_Template.prefab";
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
   #endregion
}
