using System.Linq;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

[CustomEditor(typeof(MapRoomNode))]
public class MapRoomNodeEditor: Editor
{
    GameObject _mapRoot;
    public GameObject MapRoot
    {
        get
        {
            if (_mapRoot == null)
                _mapRoot = GameObject.Find("MapRoot");
            return _mapRoot;
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 把默认的inspector的内容画出来

        MapRoomNode myScript = (MapRoomNode)target;
        if(GUILayout.Button("Reset Map Room Node"))
        {
            //设定Room的摄像机位置
            MapRoot.transform.position = new Vector3(0, 0, 100);
        }
        if(GUILayout.Button("Set Pos Data"))
        {
            if (myScript.CameraStartPos == null)
                myScript.CameraStartPos = myScript.transform.GetComponentsInChildren<Transform>
                    (true).FirstOrDefault(t => t.name == "CameraStartPos");
                
            if (myScript.RoleStartPos == null)
                myScript.RoleStartPos = myScript.transform.GetComponentsInChildren<Transform>
                    (true).FirstOrDefault(t => t.name == "RoleStartPos");
            
            //设定Room的摄像机位置
            myScript.CameraStartPos.position = new Vector3(0, 0, MapRoot.transform.position.z);
            myScript.RoleStartPos.position = new Vector3(0.067f, -0.55f, -1);
        }
        if(GUILayout.Button("Set Camera"))
        {
            //设定Room的摄像机位置
            MapRoot.transform.position = new Vector3(-myScript.CameraStartPos.position.x,
                -myScript.CameraStartPos.position.y, MapRoot.transform.position.z);
        }
    }
}