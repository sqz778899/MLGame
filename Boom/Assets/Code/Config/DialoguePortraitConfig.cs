using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialoguePortraitConfig : ScriptableObject
{
    [SerializeField]
    public List<StringSpritePair> NameToPortrait;
    Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> GetDictionary() => dict;
    
    void Awake()
    {
        SyncDictionary();  // 确保在对象启用时同步字典
    }

    void OnEnable()
    {
        //SyncDictionary();
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        SyncDictionary();
        AssetDatabase.SaveAssetIfDirty(this);
    }
#endif

    void SyncDictionary()
    {
        dict.Clear();
        foreach (var pair in NameToPortrait)
        {
            if (!dict.ContainsKey(pair.key))
                dict.Add(pair.key, pair.value);
        }
    }
    
    #region 单例
    static DialoguePortraitConfig s_instance;
    public static DialoguePortraitConfig Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<DialoguePortraitConfig>(PathConfig.DialogueNamePortraitOBJ);
            
            return s_instance;
        }
    }
    #endregion
}


[Serializable]
public class StringSpritePair
{
    public string key;
    public Sprite value;
    
    public StringSpritePair(string key, Sprite value)
    {
        this.key = key;
        this.value = value;
    }
}
