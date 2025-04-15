using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Code.Editor
{
    public class ExcelExport
    {
        [Button(ButtonSizes.Large)]
        void ExportDesignData()
        {
            ExportBullet();
            ExportItem();
            ExportGemDesign();
            ExportMuliLa();
            ExportDialogue(); //对话相关
            ExportQuestDesign();//任务相关
            ExportTalent();//天赋相关
            AssetDatabase.Refresh();
            TrunkManager.Instance.ForceRefresh();
        }

        #region 游戏设计
        public void ExportBullet()
        {
            DataSet curTables = GetDataSet();
            List<BulletJson> curBulletDesign = new List<BulletJson>();
            DataTable curTable = curTables.Tables["BulletDesign"];

            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                BulletJson curData = new BulletJson();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curData.ID = int.Parse(curTable.Rows[i][0].ToString());
                curData.Level = int.Parse(curTable.Rows[i][1].ToString());
                curData.Name = curTable.Rows[i][2].ToString();
                curData.Damage = int.Parse(curTable.Rows[i][3].ToString());
                curData.Piercing = int.Parse(curTable.Rows[i][4].ToString());
                curData.Resonance = 0;
                curData.ElementalType = int.Parse(curTable.Rows[i][5].ToString());
                curData.HitEffectName = curTable.Rows[i][6].ToString();
                curBulletDesign.Add(curData);
            }

            string content01 = JsonConvert.SerializeObject(curBulletDesign, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.BulletDesignJson, content01);
        }

        public void ExportItem()
        {
            DataSet curTables = GetDataSet();
            DataTable curTable = curTables.Tables["道具设计"];
            
            List<ItemJson> curItemDesign = new List<ItemJson>();
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                ItemJson curItem = new ItemJson();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curItem.ID = GetCellInt(curTable.Rows[i][0].ToString());
                curItem.Name = curTable.Rows[i][1].ToString();
                curItem.Rarity = GetCellInt(curTable.Rows[i][2].ToString());
                curItem.Desc = curTable.Rows[i][3].ToString();
                curItem.Price = GetCellInt(curTable.Rows[i][4].ToString());
                curItem.ResName = curTable.Rows[i][5].ToString();
                curItem.Category = ItemCategory.Equipable;
                curItemDesign.Add(curItem);
            }
            
            DataTable persistentTable = curTables.Tables["局外物品设计"];
            for (int i = 1; i < persistentTable.Rows.Count; i++)
            {
                ItemJson curItem = new ItemJson();
                if (persistentTable.Rows[i][1].ToString() == "") continue;
                curItem.ID = GetCellInt(persistentTable.Rows[i][0].ToString());
                curItem.Name = persistentTable.Rows[i][1].ToString();
                string typeStr = persistentTable.Rows[i][2].ToString();
                curItem.Desc = persistentTable.Rows[i][3].ToString();
                curItem.Price = GetCellInt(persistentTable.Rows[i][4].ToString());
                curItem.ResName = persistentTable.Rows[i][5].ToString();
                curItem.Category = ItemCategory.Persistent;
                if (typeStr == "任务道具")
                    curItem.PersistentType = PersistentItemType.QuestItem;
                if (typeStr == "养成道具")
                    curItem.PersistentType = PersistentItemType.Resource;
                curItemDesign.Add(curItem);
            }

            string content01 = JsonConvert.SerializeObject(curItemDesign, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.ItemDesignJson, content01);
        }

        public void ExportGemDesign()
        {
            DataSet curTables = GetDataSet();
            List<GemJson> curGemDesign = new List<GemJson>();
            DataTable curTable = curTables.Tables["GemDesign"];

            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                GemJson curData = new GemJson();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curData.ID = int.Parse(curTable.Rows[i][0].ToString());
                curData.Name = curTable.Rows[i][1].ToString();
                curData.Level = int.Parse(curTable.Rows[i][2].ToString());
                curData.Damage =
                    string.IsNullOrEmpty(curTable.Rows[i][3].ToString()) ? 0
                        : int.Parse(curTable.Rows[i][3].ToString());
                curData.Piercing =
                    string.IsNullOrEmpty(curTable.Rows[i][4].ToString()) ? 0
                        : int.Parse(curTable.Rows[i][4].ToString());
                curData.Resonance =
                    string.IsNullOrEmpty(curTable.Rows[i][5].ToString()) ? 0
                        : int.Parse(curTable.Rows[i][5].ToString());
                curData.Price = string.IsNullOrEmpty(curTable.Rows[i][6].ToString()) ? 0
                        : int.Parse(curTable.Rows[i][6].ToString());
                curData.ImageName = curTable.Rows[i][7].ToString();
                curGemDesign.Add(curData);
            }

            string content01 = JsonConvert.SerializeObject(curGemDesign,
                (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.GemDesignJson, content01);
        }

        public void ExportQuestDesign()
        {
            DataSet curTables = GetDataSet();
            List<QuestJson> curQuestDesign = new List<QuestJson>();
            DataTable curTable = curTables.Tables["任务设计"];
            
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                QuestJson curData = new QuestJson();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curData.ID = int.Parse(curTable.Rows[i][0].ToString());
                curData.Name = curTable.Rows[i][1].ToString();
                curData.Level = int.Parse(curTable.Rows[i][2].ToString());
                curData.Description = curTable.Rows[i][3].ToString();
                curQuestDesign.Add(curData);
            }

            string content01 = JsonConvert.SerializeObject(curQuestDesign,
                (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.QuestDesignJson, content01);

            QuestDatabaseOBJ obj = AssetDatabase.LoadAssetAtPath<QuestDatabaseOBJ>("Assets/Res/Manager/QuestDatabase.asset");
            //同步到数据库
            //已有的更新
            foreach (var each in obj.quests)
            {
                foreach (var eachJson in curQuestDesign)
                {
                    if (each.ID == eachJson.ID)
                    {
                        each.Name = eachJson.Name;
                        each.Level = eachJson.Level;
                        each.Description = eachJson.Description;
                    }
                }
            }
            //新增的添加
            foreach (var eachJson in curQuestDesign)
            {
                bool isExist = false;
                foreach (var each in obj.quests)
                {
                    if (each.ID == eachJson.ID)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    Quest newQuest = new Quest(eachJson.ID);
                    obj.quests.Add(newQuest);
                }
            }
            
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }

        public void ExportTalent()
        {
            DataSet curTables = GetDataSet();
            List<TalentJson> curTalentDesign = new List<TalentJson>();
            DataTable curTable = curTables.Tables["天赋设计"];

            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                TalentJson curData = new TalentJson();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curData.ID = GetCellInt(curTable.Rows[i][0].ToString());
                curData.Name = curTable.Rows[i][1].ToString();
                curData.Level = GetCellInt(curTable.Rows[i][2].ToString());
                curData.DependTalents = GetCellIntList(curTable.Rows[i][3].ToString());
                curData.UnlockTalents = GetCellIntList(curTable.Rows[i][4].ToString());
                curData.Price =
                    string.IsNullOrEmpty(curTable.Rows[i][5].ToString()) ? 0
                        : GetCellInt(curTable.Rows[i][5].ToString());
                curData.TalentType = (TalentEffectType)Enum.Parse(typeof(TalentEffectType), curTable.Rows[i][6].ToString());
                curData.EffectID = GetCellInt(curTable.Rows[i][7].ToString());
                curData.EffectValue = GetCellInt(curTable.Rows[i][8].ToString());
                curTalentDesign.Add(curData);
            }
            
            string content01 = JsonConvert.SerializeObject(curTalentDesign,
                (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.TalentDesignJson, content01);
        }
        #endregion

        #region 多语言

        public void ExportMuliLa()
        {
            DataSet curTables = GetMultiDataSet();
            MultiLaJson newMultiLa = new MultiLaJson();
            for (int i = 0; i < curTables.Tables.Count; i++)
            {
                DataTable curTable = curTables.Tables[i];
                for (int j = 1; j < curTable.Rows.Count; j++)
                {
                    if (curTable.Rows[j][0].ToString() == "") continue;
                    string keyStr = curTable.Rows[j][0].ToString(); //用英文当作Key
                    newMultiLa.English.Add(keyStr);
                    newMultiLa.ZH_Simplified.Add(keyStr, curTable.Rows[j][1].ToString());
                    newMultiLa.ZH_Traditional.Add(keyStr, curTable.Rows[j][2].ToString());
                    newMultiLa.Japanese.Add(keyStr, curTable.Rows[j][3].ToString());
                    newMultiLa.Korean.Add(keyStr, curTable.Rows[j][4].ToString());
                    //curTable.Rows[j][0]
                }
            }

            string content = JsonConvert.SerializeObject(newMultiLa, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.MultiLaDesignJson, content);
        }

        DataSet GetMultiDataSet()
        {
            FileStream fileStream =
                File.Open(GetDesignExcelPath("Multi_Language.xlsx"), FileMode.Open, FileAccess.Read);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
            DataSet result = excelDataReader.AsDataSet();
            return result;
        }

        #endregion

        #region 对话相关
        void SetPerDialoguesData(ref Dictionary<string,List<DiaSingle>> curDict ,DataTable curTable)
        {
            List<DiaSingle> dialogues = new List<DiaSingle>();
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                DiaSingle curData = new DiaSingle();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curData.Sign = curTable.Rows[i][0].ToString();
                curData.ID = int.Parse(curTable.Rows[i][1].ToString());
                curData.Name = curTable.Rows[i][2].ToString();
                curData.IsLeft = int.Parse(curTable.Rows[i][3].ToString());
                curData.NextIdex = int.Parse(curTable.Rows[i][4].ToString());
                curData.Content = curTable.Rows[i][5].ToString();
                dialogues.Add(curData);
            }
            curDict[curTable.TableName] = dialogues;
        }
        
        void ExportDialogue()
        {
            
            Dictionary<string,List<DiaSingle>> curDiaDesignDict = new Dictionary<string, List<DiaSingle>>();
            
            DataSet curTableTutorial = GetDataSet("BeginnerTutorial.xlsx");
            for (int i = 0; i < curTableTutorial.Tables.Count; i++)
                SetPerDialoguesData(ref curDiaDesignDict,curTableTutorial.Tables[i]);
            
            DataSet curTableStoryline = GetDataSet("StorylineDia.xlsx");
            for (int i = 0; i < curTableStoryline.Tables.Count; i++)
                SetPerDialoguesData(ref curDiaDesignDict,curTableStoryline.Tables[i]);

            string content01 = JsonConvert.SerializeObject(curDiaDesignDict, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.DialogueDesignJson, content01);
        }
        #endregion

        #region 不关心的私有方法
        DataSet GetDataSet(string excelFileName = "CommonDesign.xlsx")
        {
            FileStream fileStream = File.Open(GetDesignExcelPath(excelFileName), FileMode.Open, FileAccess.Read);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
            DataSet result = excelDataReader.AsDataSet();
            return result;
        }
        
        List<int> GetCellIntList(string curStr)
        {
            if (curStr == "")
                return new List<int>();
            else
                return curStr.Split(';').Select(int.Parse).ToList();
        }
        
        int GetCellInt(string curStr)
        {
            if (curStr == "")
                return 0;
            else
                return int.Parse(curStr);
        }
        
        string GetDesignExcelPath(string excelName)
        {
            string diskDir = Application.streamingAssetsPath.Replace(
                "Assets/StreamingAssets", "Excel/") + excelName;
            return diskDir;
        }
        #endregion
    }
}