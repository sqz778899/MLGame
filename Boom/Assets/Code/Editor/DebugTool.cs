using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DebugTool
{
    public GameObject Root;
    public Material _defaultMaterial;

    [Button(ButtonSizes.Large)]
    void DealMissingScript()
    {
        SpriteRenderer[] allTrans = Root.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var each in allTrans)
        {
            each.material = _defaultMaterial;
        }
        //GameObjectUtility.RemoveMonoBehavioursWithMissingScript()
    }

    [Button(ButtonSizes.Large)]
    void LogSS()
    {
        string path = "Assets/Res/Character/Image/Enemy/Enemy_Portrait_01.png";
        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        Debug.Log(s.name);
    }
}
