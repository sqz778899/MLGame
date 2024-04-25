using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;

public class ExcelExport
{
    [Button(ButtonSizes.Large)]
    void ExportDesignData()
    {
        ExportBullet();
        ExportBuffDesign();
        ExportLevelBuffDesign();
        ExportMuliLa();
        ExportRoleDesign();
    }

    #region 游戏设计
    public void ExportBullet()
    {
        DataSet curTables = GetDataSet();
        List<BulletDataJson> curBulletDesign = new List<BulletDataJson>();
        DataTable curTable = curTables.Tables[0];
       
        for (int i = 1; i < curTable.Rows.Count; i++)
        {
            BulletDataJson curData = new BulletDataJson();
            if (curTable.Rows[i][1].ToString() == "") continue;
            curData.ID = int.Parse(curTable.Rows[i][0].ToString());
            curData.Level = int.Parse(curTable.Rows[i][1].ToString());
            curData.name = curTable.Rows[i][2].ToString();
            curData.damage = int.Parse(curTable.Rows[i][3].ToString());
            curData.penetration = int.Parse(curTable.Rows[i][4].ToString());
            curData.elementalType = int.Parse(curTable.Rows[i][5].ToString());
            curData.hitEffectName = curTable.Rows[i][6].ToString();
            curBulletDesign.Add(curData);
        }
        
        string content01 = JsonConvert.SerializeObject(curBulletDesign,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.BulletDesignJson, content01);
    }

    public void ExportBuffDesign()
    {
        DataSet curTables = GetDataSet();
        List<BuffDataJson> curBuffData = new List<BuffDataJson>();
        
        DataTable curTable = curTables.Tables[1];
        for (int i = 1; i < curTable.Rows.Count; i++)
        {
            BuffDataJson curData = new BuffDataJson();
            CommonAttribute comAttri = new CommonAttribute();
            SpeAttribute speAttri = new SpeAttribute();
            if (curTable.Rows[i][1].ToString() == "") continue;
            curData.ID = int.Parse(curTable.Rows[i][0].ToString());
            curData.name = curTable.Rows[i][1].ToString();
            curData.rare = int.Parse(curTable.Rows[i][2].ToString());
            comAttri.damage = int.Parse(curTable.Rows[i][3].ToString());
            comAttri.elementalType = int.Parse(curTable.Rows[i][4].ToString());
            comAttri.elementalValue = int.Parse(curTable.Rows[i][5].ToString());
            comAttri.Penetration = int.Parse(curTable.Rows[i][6].ToString());
            speAttri.interest = int.Parse(curTable.Rows[i][7].ToString());
            speAttri.standbyAdd = int.Parse(curTable.Rows[i][8].ToString());
            curData.comAttributes = comAttri;
            curData.speAttributes = speAttri;
            curBuffData.Add(curData);
        }
        
        string content01 = JsonConvert.SerializeObject(curBuffData,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.BuffDesignJson, content01);
    }

    public void ExportLevelBuffDesign()
    {
        DataSet curTables = GetDataSet();
        List<LevelBuff> curLBuffData = new List<LevelBuff>();

        DataTable curTable = curTables.Tables[2];
        for (int i = 1; i < curTable.Rows.Count; i++)
        {
            LevelBuff curLB = new LevelBuff();
            List<RollProbability> curRProb = new List<RollProbability>();
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
                RollProbability curRP = new RollProbability();
                curRP.ID = int.Parse(sBuffIDTemp[j]);
                curRP.Probability = normalizeProb[j];
                curRProb.Add(curRP);
            }
            curLB.CurBuffProb = curRProb;
            curLBuffData.Add(curLB);
        }
        
        string content01 = JsonConvert.SerializeObject(curLBuffData,(Formatting) Formatting.Indented);
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
        
        string content = JsonConvert.SerializeObject(curRoleData,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.RoleDesignJson, content);
    }
    
    DataSet GetDataSet()
    {
        FileStream fileStream = File.Open(GetDesignExcelPath("CommonDesign.xlsx"), FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        return result;
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
                newMultiLa.ZH_Simplified.Add(keyStr,curTable.Rows[j][1].ToString());
                newMultiLa.ZH_Traditional.Add(keyStr,curTable.Rows[j][2].ToString());
                newMultiLa.Japanese.Add(keyStr,curTable.Rows[j][3].ToString());
                newMultiLa.Korean.Add(keyStr,curTable.Rows[j][4].ToString());
                //curTable.Rows[j][0]
            }
        }
        string content = JsonConvert.SerializeObject(newMultiLa,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.MultiLaDesignJson, content);
    }
    
    DataSet GetMultiDataSet()
    {
        FileStream fileStream = File.Open(GetDesignExcelPath("Multi_Language.xlsx"), FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        return result;
    }
    #endregion
    string GetDesignExcelPath(string excelName)
    {
        string diskDir = Application.streamingAssetsPath.Replace(
            "Assets/StreamingAssets", "Excel/") + excelName;
        return diskDir;
    }
}