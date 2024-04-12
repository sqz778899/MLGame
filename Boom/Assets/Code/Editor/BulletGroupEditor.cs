using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlotIDCalculate))]
public class BulletGroupEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SlotIDCalculate myScript = (SlotIDCalculate)target;
        if(GUILayout.Button("InitSlotID"))
        {
            Debug.Log("InitSlotID");
            myScript.InitSlotID();
        }
    }
}