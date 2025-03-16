#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
[CanEditMultipleObjects]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("GenerateNodeIDs"))
        {
            var mtarget = target as MapManager;
            //mtarget.InitMapData();
            mtarget.SetAllRoomIDs();
        }
    }
}
#endif
