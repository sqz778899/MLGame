using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class MultiLa :ScriptableObject
{
    #region 单例
    static MultiLa s_instance;
    
    public static MultiLa Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<MultiLa>(PathConfig.MultiLaOBJ);
            return s_instance;
        }
    }
    #endregion

    public MultiLaEN CurLanguage;
    
    public void ChangeLanguage(int value)
    {
        CurLanguage = (MultiLaEN)value;
    }
    
    public string LocalizeText(string text,Dictionary<string, string> dict)
    {
        return Regex.Replace(text, @"\b\w+\x20?\w+\b", match =>
        {
            string word = match.Value;
            if (dict.TryGetValue(word, out string localizedWord))
            {
                return localizedWord;
            }
            else
            {
                return word;
            }
        });
    }
    
    public void GetMLStr(string OrginStr,float OrginFondSize,ref MStr curMStr)
    {
        string rStr = OrginStr;
        float rFondSize = curMStr.FondSize;
        TMP_FontAsset rAsset = null;
        switch (CurLanguage)
        {
            case MultiLaEN.English:
                rAsset = GetAsset(MultiLaEN.ZH_Simplified);
                //curMStr.Str = curStr;
                //curMStr.FondSize
                break;
            case MultiLaEN.ZH_Simplified:
                rStr = LocalizeText(OrginStr, multiLaJsons.ZH_Simplified);
                rAsset = GetAsset(MultiLaEN.ZH_Simplified);
                break;
            case MultiLaEN.ZH_Traditional:
                rStr = LocalizeText(OrginStr, multiLaJsons.ZH_Traditional);
                rAsset = GetAsset(MultiLaEN.ZH_Traditional);
                break;
            case MultiLaEN.Japanese:
                rStr = LocalizeText(OrginStr, multiLaJsons.Japanese);
                rAsset = GetAsset(MultiLaEN.Japanese);
                rFondSize = OrginFondSize - 7;
                break;
            case MultiLaEN.Korean:
                rStr = LocalizeText(OrginStr, multiLaJsons.Korean);
                rAsset = GetAsset(MultiLaEN.Korean);
                rFondSize = OrginFondSize - 5;
                break;
        }

        curMStr.Str = rStr;
        curMStr.FondAsset = rAsset;
        curMStr.FondSize = rFondSize;
    }

    TMP_FontAsset GetAsset(MultiLaEN MultiLa)
    {
        return ResManager.instance.GetAssetCache<TMP_FontAsset>(PathConfig.GetFondPath(MultiLa));
    }
    #region 初始化文本数据
    MultiLaJson _multiLaJsons;
    public MultiLaJson multiLaJsons
    {
        get
        {
            if (_multiLaJsons == null)
                _multiLaJsons = LoadMultiLaData();
            return _multiLaJsons;
        }
    }
    
    MultiLaJson LoadMultiLaData()
    {
        string MultiLaString = File.ReadAllText(PathConfig.MultiLaDesignJson);
        MultiLaJson MultiLaDataJsons = JsonConvert.DeserializeObject<MultiLaJson>(MultiLaString);
        return MultiLaDataJsons;
    }
    #endregion
    //GlobalSetting
    //GetMLStr("Play")
}
