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
    }

    public void ExportBullet()
    {
        DataSet curTables = GetDataSet();
        
        List<BulletDataJson> curBulletDesign = new List<BulletDataJson>();
        int rowsPrefix = curTables.Tables[0].Rows.Count;
        for (int i = 1; i < rowsPrefix; i++)
        {
            BulletDataJson curData = new BulletDataJson();
            if (curTables.Tables[0].Rows[i][1].ToString() == "") continue;
            curData.ID = int.Parse(curTables.Tables[0].Rows[i][0].ToString());
            curData.Level = int.Parse(curTables.Tables[0].Rows[i][1].ToString());
            curData.name = curTables.Tables[0].Rows[i][2].ToString();
            curData.damage = int.Parse(curTables.Tables[0].Rows[i][3].ToString());
            curData.elementalType = int.Parse(curTables.Tables[0].Rows[i][4].ToString());
            curData.hitEffectName = curTables.Tables[0].Rows[i][5].ToString();
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
            curData.ID = int.Parse(curTables.Tables[0].Rows[i][0].ToString());
            curData.name = curTables.Tables[0].Rows[i][1].ToString();
            comAttri.damage = int.Parse(curTables.Tables[0].Rows[i][2].ToString());
            comAttri.elementalType = int.Parse(curTables.Tables[0].Rows[i][3].ToString());
            comAttri.elementalValue = int.Parse(curTables.Tables[0].Rows[i][4].ToString());
            comAttri.Penetration = int.Parse(curTables.Tables[0].Rows[i][5].ToString());
            speAttri.interest = int.Parse(curTables.Tables[0].Rows[i][6].ToString());
            speAttri.standbyAdd = int.Parse(curTables.Tables[0].Rows[i][7].ToString());
            curData.comAttributes = comAttri;
            curData.speAttributes = speAttri;
            curBuffData.Add(curData);
        }
    }

    DataSet GetDataSet()
    {
        FileStream fileStream = File.Open(GetBulletDesignPath("CommonDesign.xlsx"), FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();
        return result;
    }
    string GetBulletDesignPath(string excelName)
    {
        string diskDir = Application.streamingAssetsPath.Replace(
            "Assets/StreamingAssets", "Excel/") + excelName;
        return diskDir;
    }
}