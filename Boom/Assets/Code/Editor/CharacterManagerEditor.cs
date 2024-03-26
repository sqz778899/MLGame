using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterManager))]
public class CharacterManagerEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 把默认的inspector的内容画出来

        CharacterManager myScript = (CharacterManager)target;
        if(GUILayout.Button("SaveFile"))
        {
            myScript.LoadSaveFile();
        }
    }
}