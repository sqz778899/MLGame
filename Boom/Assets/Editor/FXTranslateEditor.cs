using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class FXTranslateEditor
{
    [Button(ButtonSizes.Large)]
    void ppp()
    {
        foreach (var each in Selection.gameObjects)
        {
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(each);
            string[] DepStrs = AssetDatabase.GetDependencies(path);
        }
        Debug.Log("xxx");
    }
}
