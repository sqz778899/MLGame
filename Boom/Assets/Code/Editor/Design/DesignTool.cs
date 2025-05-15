using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DesignTool
{
    #region 存档测试
    [TitleGroup("存档测试")] [PropertyOrder(-1)]
    public int 存档测试;
    [Button("默认存档",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("默认存档",0.5f)]
    void ResetAll() => TrunkManager.Instance.SetSaveFileTemplate();
    
    [Button("默认存档万解",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("默认存档",0.5f)]
    void ResetAllFiveSlots() => TrunkManager.Instance.SetSaveFileFiveSlotsTemplate();
    
    [Button("存档",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("存档",0.5f)]
    void SaveFile() => SaveManager.SaveFile();
    
    [Button("读档",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("存档",0.5f)]
    void LoadFile() => SaveManager.LoadSaveFile();
    #endregion
    
    [TitleGroup("多语言测试")] [PropertyOrder(1)]
    public int 多语言测试;
    [Button("中文",ButtonSizes.Large),PropertyOrder(1)]
    [HorizontalGroup("多语言测试G",0.333f)]
    void SetLanguageChinese() => Loc.Load(LanguageType.ChineseSimplified);
    
    [Button("英语",ButtonSizes.Large),PropertyOrder(1)]
    [HorizontalGroup("多语言测试G",0.333f)]
    void SetLanguageEnglish() => Loc.Load(LanguageType.English);
    
    [Button("日语",ButtonSizes.Large),PropertyOrder(1)]
    [HorizontalGroup("多语言测试G",0.333f)]
    void SetLanguageJapanese() => Loc.Load(LanguageType.Japanese);
}