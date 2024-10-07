#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapLogic))]
[CanEditMultipleObjects]
public class MapLogicEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("GenerateNodeIDs"))
        {
            var mtarget = target as MapLogic;
            mtarget.InitMapData();
            mtarget.SetAllIDs();
        }
    }
}
#endif
