﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainRoleManager))]
public class CharacterManagerEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 把默认的inspector的内容画出来

        MainRoleManager myScript = (MainRoleManager)target;
        if(GUILayout.Button("dddd"))
        {
            //myScript.LoadSaveFile();
        }
    }
}