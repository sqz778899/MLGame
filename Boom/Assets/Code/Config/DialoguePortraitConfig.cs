using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDictionaryConfig", menuName = "Config/DictionaryConfig")]
public class DialoguePortraitConfig : ScriptableObject
{
    [SerializeField]
    public List<StringSpritePair> NameToPortrait;
    
    
    private Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> GetDictionary()
    {
        return dict;
    }

    void OnEnable()
    {
        SyncDictionary();
    }

    void OnValidate()
    {
        SyncDictionary();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
#endif
    }

    void SyncDictionary()
    {
        dict.Clear();
        foreach (var pair in NameToPortrait)
        {
            if (!dict.ContainsKey(pair.key))
                dict.Add(pair.key, pair.value);
        }
    }
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
