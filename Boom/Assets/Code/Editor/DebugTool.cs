using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugTool
{
    public GameObject Root;
    public Material _defaultMaterial;

    [Button("解决脚本Miss",ButtonSizes.Large)]
    [ButtonGroup("S")]
    void DealMissingScript()
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(Root);
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("替换材质球")]
    void DealMat()
    {
        SpriteRenderer[] allTrans = Root.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var each in allTrans)
        {
            each.material = _defaultMaterial;
        }
    }
}
