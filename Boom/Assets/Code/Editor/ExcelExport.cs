using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;

namespace Code.Editor
{
    public class ExcelExport
    {
        [Button(ButtonSizes.Large)]
        void ExportDesignData()
        {
            ExportBullet();
            ExportItem();
            ExportLevelBuffDesign();
            ExportMuliLa();
            ExportRoleDesign();
            ExportPREventDesign();
            ExportGemDesign();
            ExportDialogue(); //对话相关
            ExportQuestDesign();//任务相关
            AssetDatabase.Refresh();
        }

        #region 游戏设计
        public void ExportBullet()
        {
            DataSet curTables = GetDataSet();
            List<BulletJson> curBulletDesign = new List<BulletJson>();
            DataTable curTable = curTables.Tables[0];

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
            DataTable curTable = curTables.Tables.Cast<DataTable>()
                .FirstOrDefault(t => t.TableName == "ItemDesign");

            if (curTable == null)
            {
                Debug.LogError("没有找到\"ItemDesign\"表");
                return;
            }

            List<ItemJson> curItemDesign = new List<ItemJson>();
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                ItemJson curItem = new ItemJson();
                var s = curTable.Rows[i][1];
                if (curTable.Rows[i][1].ToString() == "") continue;
                curItem.ID = int.Parse(curTable.Rows[i][0].ToString());
                curItem.Rare = int.Parse(curTable.Rows[i][1].ToString());
                curItem.Name = curTable.Rows[i][2].ToString();
                curItem.Attribute.waterElement = GetCellInt(curTable.Rows[i][3].ToString());
                curItem.Attribute.fireElement = GetCellInt(curTable.Rows[i][4].ToString());
                curItem.Attribute.thunderElement = GetCellInt(curTable.Rows[i][5].ToString());
                curItem.Attribute.lightElement = GetCellInt(curTable.Rows[i][6].ToString());
                curItem.Attribute.darkElement = GetCellInt(curTable.Rows[i][7].ToString());
                //
                curItem.Attribute.extraWaterDamage = GetCellInt(curTable.Rows[i][8].ToString());
                curItem.Attribute.extraFireDamage = GetCellInt(curTable.Rows[i][9].ToString());
                curItem.Attribute.extraThunderDamage = GetCellInt(curTable.Rows[i][10].ToString());
                curItem.Attribute.extraLightDamage = GetCellInt(curTable.Rows[i][11].ToString());
                curItem.Attribute.extraDarkDamage = GetCellInt(curTable.Rows[i][12].ToString());

                curItem.Attribute.maxDamage = GetCellInt(curTable.Rows[i][13].ToString());

                curItem.ImageName = curTable.Rows[i][14].ToString();
                curItemDesign.Add(curItem);
            }

            string content01 = JsonConvert.SerializeObject(curItemDesign, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.ItemDesignJson, content01);
        }

        int GetCellInt(string curStr)
        {
            if (curStr == "")
                return 0;
            else
                return int.Parse(curStr);
        }
        
        public void ExportLevelBuffDesign()
        {
            DataSet curTables = GetDataSet();
            List<LevelBuff> curLBuffData = new List<LevelBuff>();

            DataTable curTable = curTables.Tables[2];
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                LevelBuff curLB = new LevelBuff();
                List<RollPR> curRProb = new List<RollPR>();
                if (curTable.Rows[i][1].ToString() == "") continue;
                curLB.LevelID = int.Parse(curTable.Rows[i][0].ToString());

                string sBuffIDs = curTable.Rows[i][1].ToString();
                string sBuffProbs = curTable.Rows[i][2].ToString();
                string[] sBuffIDTemp = sBuffIDs.Split(";");
                string[] sBuffProbTemp = sBuffProbs.Split(";");
                if (sBuffIDTemp.Length != sBuffProbTemp.Length)
                {
                    Debug.LogError("导表失败");
                    return;
                }

                List<float> orProb = new List<float>();
                for (int j = 0; j < sBuffIDTemp.Length; j++)
                    orProb.Add(float.Parse(sBuffProbTemp[j]));

                List<float> normalizeProb = RollManager.Instance.NormalizeProb(orProb);
                for (int j = 0; j < sBuffIDTemp.Length; j++)
                {
                    RollPR curRP = new RollPR();
                    curRP.ID = int.Parse(sBuffIDTemp[j]);
                    curRP.Probability = normalizeProb[j];
                    curRProb.Add(curRP);
                }

                curLB.CurBuffProb = curRProb;
                curLBuffData.Add(curLB);
            }

            string content01 = JsonConvert.SerializeObject(curLBuffData, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.LevelBuffDesignJson, content01);
        }

        public void ExportRoleDesign()
        {
            DataSet curTables = GetDataSet();
            List<RoleBase> curRoleData = new List<RoleBase>();

            DataTable curTable = curTables.Tables[3];
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                RoleBase curRole = new RoleBase();
                RoleAttri curRoleAttri = new RoleAttri();
                curRole.Attri = curRoleAttri;
                if (curTable.Rows[i][0].ToString() == "") continue;

                curRole.ID = int.Parse(curTable.Rows[i][0].ToString());
                //...................RoleAttri........................
                curRoleAttri.StandbyAdd = int.Parse(curTable.Rows[i][1].ToString());
                //....................................................
                curRole.BloodGroup = curTable.Rows[i][2].ToString();
                curRole.ZodiacSign = curTable.Rows[i][3].ToString();
                curRole.MBTI = curTable.Rows[i][4].ToString();
                curRole.Description = curTable.Rows[i][5].ToString();
                curRoleData.Add(curRole);
            }

            string content = JsonConvert.SerializeObject(curRoleData, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.RoleDesignJson, content);
        }

        public void ExportPREventDesign()
        {
            DataSet curTables = GetDataSet();
            List<RollPREvent> curPREvents = new List<RollPREvent>();

            DataTable curTable = curTables.Tables[4];
            for (int i = 1; i < curTable.Rows.Count; i++)
            {
                RollPREvent curRollPREvent = new RollPREvent();
                if (curTable.Rows[i][0].ToString() == "") continue;

                curRollPREvent.ID = int.Parse(curTable.Rows[i][0].ToString());
                //...................Add........................
                Dictionary<int, float> AddPRDict = new Dictionary<int, float>();
                List<float> curAddPR = ExcellUtility.GetListFloat(curTable.Rows[i][1].ToString());
                List<int> curAddPRBullet = ExcellUtility.GetListInt(curTable.Rows[i][2].ToString());
                for (int j = 0; j < curAddPR.Count; j++)
                    AddPRDict[curAddPRBullet[j]] = curAddPR[j];
                curRollPREvent.AddPRDict = AddPRDict;
                //...................Sub........................
                Dictionary<int, float> SubPRDict = new Dictionary<int, float>();
                List<float> curSubPR = ExcellUtility.GetListFloat(curTable.Rows[i][3].ToString());
                List<int> curSubPRBullet = ExcellUtility.GetListInt(curTable.Rows[i][4].ToString());
                for (int j = 0; j < curSubPR.Count; j++)
                    SubPRDict[curSubPRBullet[j]] = curSubPR[j];
                curRollPREvent.SubPRDict = SubPRDict;
                //...................Title........................
                curRollPREvent.Title = curTable.Rows[i][5].ToString();
                curRollPREvent.EDescription = curTable.Rows[i][6].ToString();
                curPREvents.Add(curRollPREvent);
            }

            string content = JsonConvert.SerializeObject(curPREvents, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.PREventDesignJson, content);
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
            DataSet curTables = GetDataSet("BeginnerTutorial.xlsx");
            
            Dictionary<string,List<DiaSingle>> curDiaDesignDict = new Dictionary<string, List<DiaSingle>>();
            for (int i = 0; i < curTables.Tables.Count; i++)
                SetPerDialoguesData(ref curDiaDesignDict,curTables.Tables[i]);

            string content01 = JsonConvert.SerializeObject(curDiaDesignDict, (Formatting)Formatting.Indented);
            File.WriteAllText(PathConfig.DialogueDesignJson, content01);
        }
        #endregion

        DataSet GetDataSet(string excelFileName = "CommonDesign.xlsx")
        {
            FileStream fileStream = File.Open(GetDesignExcelPath(excelFileName), FileMode.Open, FileAccess.Read);
            IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
            DataSet result = excelDataReader.AsDataSet();
            return result;
        }
        
        string GetDesignExcelPath(string excelName)
        {
            string diskDir = Application.streamingAssetsPath.Replace(
                "Assets/StreamingAssets", "Excel/") + excelName;
            return diskDir;
        }
    }
}