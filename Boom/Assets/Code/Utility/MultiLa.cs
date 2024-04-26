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
        //..................短语............................
        Regex rex01 = new Regex(@"\b.*\b");
        string shortWord = rex01.Match(text).Value;
        dict.TryGetValue(shortWord, out string localizedWord);
        if (localizedWord != null)
        {
            return text.Replace(shortWord, localizedWord);
        }
        
        //..................如果短语匹配不到 换替换词匹配算法...........................
        Regex rex = new Regex("[A-Za-z]+");
        var curMatches = rex.Matches(text);
        string newStr = "";
        for (int i = 0; i < curMatches.Count; i++)
        {
            dict.TryGetValue(curMatches[i].Value, out string word);
            if (word != null)
                newStr = text.Replace(curMatches[i].Value, word);
        }

        return newStr;
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
                rFondSize = OrginFondSize;
                //curMStr.FondSize
                break;
            case MultiLaEN.ZH_Simplified:
                rStr = LocalizeText(OrginStr, multiLaJsons.ZH_Simplified);
                rAsset = GetAsset(MultiLaEN.ZH_Simplified);
                rFondSize = OrginFondSize;
                break;
            case MultiLaEN.ZH_Traditional:
                rStr = LocalizeText(OrginStr, multiLaJsons.ZH_Traditional);
                rAsset = GetAsset(MultiLaEN.ZH_Traditional);
                rFondSize = OrginFondSize;
                break;
            case MultiLaEN.Japanese:
                rStr = LocalizeText(OrginStr, multiLaJsons.Japanese);
                rAsset = GetAsset(MultiLaEN.Japanese);
                rFondSize = OrginFondSize - 5;
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

    public void SyncText(TextMeshProUGUI curPro,string curStr)
    {
        SetStr curSC = curPro.gameObject.GetComponent<SetStr>();
        if (curSC == null)
        {
            curPro.text = curStr;
            return;
        }

        curSC._orginStr = curStr;
        curSC.SyncTextData();
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
