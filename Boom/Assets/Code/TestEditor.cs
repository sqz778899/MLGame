#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test))]
[CanEditMultipleObjects]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("GenerateData"))
        {
            var mtarget = target as Test;
            mtarget.GenerateMisc();
        }
    }
   
}   
#endif