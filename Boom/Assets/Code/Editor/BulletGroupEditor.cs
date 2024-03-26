using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletGroup))]
public class BulletGroupEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BulletGroup myScript = (BulletGroup)target;
        if(GUILayout.Button("InitSlotID"))
        {
            Debug.Log("InitSlotID");
            myScript.InitSlotID();
        }
    }
}