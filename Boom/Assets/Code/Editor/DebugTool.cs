using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

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

    public Transform TestTrans;
    [Button(ButtonSizes.Large)]
    void LogSS()
    {
        Debug.Log($"WorldPos {TestTrans.position}");
        Debug.Log($"LocalPos {TestTrans.localPosition}");
    }
}
