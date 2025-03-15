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

    [Button(ButtonSizes.Large)]
    void DealMissingScript()
    {
        /*
        SpriteRenderer[] allTrans = Root.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var each in allTrans)
        {
            each.material = _defaultMaterial;
        }
        */

        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(Root);
    }

    
    [Button("主界面",ButtonSizes.Large)]
    [ButtonGroup("Scene")]
    void LogSceneMain()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/1.MainEnv.unity");
    }
    
    [Button("关卡",ButtonSizes.Large)]
    [ButtonGroup("Scene")]
    void LogSceneLevel()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/2.LevelScene.unity");
    }
    
    [Button("开始游戏",ButtonSizes.Large)]
    [ButtonGroup("Scene")]
    void LogSceneStart()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/0.StartGame.unity");
    }
}
