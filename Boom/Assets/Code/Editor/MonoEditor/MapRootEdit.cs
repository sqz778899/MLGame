using System.Linq;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

[CustomEditor(typeof(MapMouseControl))]
public class MapRootEdit: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 把默认的inspector的内容画出来

        MapMouseControl myScript = (MapMouseControl)target;
        if (GUILayout.Button("Set Room ID"))
        {
            MapRoomNode[] rooms = myScript.gameObject.
                GetComponentsInChildren<MapRoomNode>(true);
            for (int i = 0; i < rooms.Length; i++)
            {
                rooms[i].RoomID = i + 1;
            }
        }
    }
}