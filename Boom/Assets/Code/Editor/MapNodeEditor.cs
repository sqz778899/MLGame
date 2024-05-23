using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapNode))]
public class MapNodeEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 把默认的inspector的内容画出来

        MapNode myScript = (MapNode)target;
        if(GUILayout.Button("Change Node State"))
        {
            myScript.ChangeState();
        }
    }
}