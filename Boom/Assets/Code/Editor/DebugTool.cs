using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DebugTool
{
    public GameObject Root;

    [Button(ButtonSizes.Large)]
    void DealMissingScript()
    {
        Transform[] allTrans = Root.GetComponentsInChildren<Transform>(true);
        foreach (var each in allTrans)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(each.gameObject);
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
